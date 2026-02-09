using Investment.Application.Utilities.Abstractions;
using Investment.Application.Utilities.Exceptions;
using Investment.Domain.Transactions.Entities;
using Investment.Domain.Transactions.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Investment.Application.CryptoPayment.Commands.WithdrawalVerifyByAdmin;

public class WithdrawalVerifyByAdminCommandHandler(
    IUserBalanceCommandRepository userBalanceRepository,
    IUnitOfWork unitOfWork,
    ICryptoPaymentsService cryptoPaymentsService)
    : IRequestHandler<WithdrawalVerifyByAdminCommand>
{
    public async Task Handle(WithdrawalVerifyByAdminCommand request, CancellationToken cancellationToken)
    {       
        var userBalance = await userBalanceRepository.GetByUserIdWithSpecificTransactionAsync(
            userId: request.UserId,
            transactionId: request.TransactionId, cancellationToken)
            ?? throw new EntityNotFoundException<UserBalance>();

        var transaction = userBalance.VerifyWithdrawalTransaction(request.TransactionId);

        try
        {
            await cryptoPaymentsService.VerifyWithdrawalAsync(
                  BatchWithdrawalId: transaction.Reason,
                  VerificationCode: request.VerificationCode,
                  cancellationToken
                  );
        }
        catch(Exception ex)
        {
            throw new InvalidCommandException("Verification failed", ex.Message , ErrorCodeEnum.None);
        }
  
        await unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
