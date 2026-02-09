using Investment.Application.Utilities.Abstractions;
using Investment.Application.Utilities.Exceptions;
using Investment.Domain.Transactions.Entities;
using Investment.Domain.Transactions.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Investment.Application.CryptoPayment.Commands.WithdrawalApproveByAdmin;

public class WithdrawalApproveByAdminCommandHandler(
        IUserBalanceCommandRepository userBalanceRepository,
        IUnitOfWork unitOfWork)
    : IRequestHandler<WithdrawalApproveByAdminCommand>
{
    public async Task Handle(WithdrawalApproveByAdminCommand command, CancellationToken cancellationToken)
    {

        UserBalance? userBalance = await userBalanceRepository.GetByUserIdWithSpecificTransactionAsync(userId: command.UserId,
            transactionId: command.TransactionId,
            cancellationToken) ?? throw new EntityNotFoundException<UserBalance>(); 

        userBalance.ApproveWithdrawalTransaction(command.TransactionId);

        await unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
