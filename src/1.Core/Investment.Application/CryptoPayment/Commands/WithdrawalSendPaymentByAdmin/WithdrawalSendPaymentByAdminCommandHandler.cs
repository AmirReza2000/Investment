using Investment.Application.CryptoPayment.Commands.WithdrawalSendPaymentByAdmin;
using Investment.Application.Utilities.Abstractions;
using Investment.Application.Utilities.Exceptions;
using Investment.Domain.Transactions.Entities;
using Investment.Domain.Transactions.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Investment.Application.CryptoPayment.Commands.WithdrawalSendPaymentByAdmin;

public class WithdrawalSendPaymentByAdminCommandHandler(
    IUserBalanceCommandRepository userBalanceCommandRepository,
    IUnitOfWork unitOfWork,
    ICryptoPaymentsService cryptoPaymentsService
    )
    : IRequestHandler<WithdrawalSendPaymentByAdminCommand, WithdrawalSendPaymentByAdminResponse>
{
    public async Task<WithdrawalSendPaymentByAdminResponse> Handle(WithdrawalSendPaymentByAdminCommand request, CancellationToken cancellationToken)
    {
        var userBalance = await userBalanceCommandRepository.GetByUserIdWithSpecificTransactionAsync(
            request.UserId,
            request.TransactionId,
            cancellationToken
        ) ?? throw new EntityNotFoundException<UserBalance>();

        var transaction = userBalance.SendWithdrawalPayment(request.TransactionId);

        await cryptoPaymentsService.AssertApiIsAvailableAsync(cancellationToken);

        var response = await cryptoPaymentsService.WithdrawalRequestAsync(transaction.Amount,
              address: transaction.Address,
              description: $"create a withdrawal with userId:{userBalance.UserId} and transactionId:{transaction.Id}.",
              userId: request.UserId,
              cancellationToken);

        userBalance.AssignReasonForWithdrawal(
           id: transaction.Id,
            reason: response.Withdrawals.Single().BatchWithdrawalId);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return
            new WithdrawalSendPaymentByAdminResponse
            {
                PaymentId = response.Withdrawals.First().BatchWithdrawalId,
                AmountCurrency = response.Withdrawals.First().Amount
            };
    }
}
