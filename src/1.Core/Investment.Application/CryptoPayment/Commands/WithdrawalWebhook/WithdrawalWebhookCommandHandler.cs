using System;
using System.Collections.Generic;
using System.Text;
using Investment.Application.CryptoPayment.Commands.DepositWebhook;
using Investment.Application.Utilities.Abstractions;
using Investment.Application.Utilities.Exceptions;
using Investment.Domain.Transactions.Entities;
using Investment.Domain.Transactions.Enums.Transaction;
using Investment.Domain.Transactions.Repositories;
using MediatR;

namespace Investment.Application.CryptoPayment.Commands.WithdrawalWebhook;

public class WithdrawalWebhookCommandHandler(
    IUserBalanceCommandRepository userBalanceRepository,
    IUnitOfWork unitOfWork)
    : IRequestHandler<BatchWithdrawalWebhookCommand>
{
    public async Task Handle(BatchWithdrawalWebhookCommand request, CancellationToken cancellationToken)
    {

        UserBalance userBalance =
            await userBalanceRepository.GetUserBalanceWithTransactionAsync(
                userId: request.UserId,
                reason: request.BatchWithdrawalId,
                reasonType: ReasonTypeEnum.NowPayment,
                cancellationToken)
            ?? throw new EntityNotFoundException<UserBalance>();

        userBalance.UpdateWithdrawalStatus(
            Enum.Parse<WithdrawalStatusEnum>(request.Status, true),
            reason: request.BatchWithdrawalId);

        await unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
