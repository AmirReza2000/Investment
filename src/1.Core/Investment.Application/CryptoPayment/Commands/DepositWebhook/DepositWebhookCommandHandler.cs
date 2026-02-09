using Investment.Application.Utilities.Abstractions;
using Investment.Application.Utilities.Exceptions;
using Investment.Domain.Transactions.Entities;
using Investment.Domain.Transactions.Enums.Transaction;
using Investment.Domain.Transactions.Repositories;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Text;

namespace Investment.Application.CryptoPayment.Commands.DepositWebhook;

public class DepositWebhookCommandHandler(
    IUserBalanceCommandRepository transactionRepository,
    IUnitOfWork unitOfWork)
    : IRequestHandler<DepositWebhookCommand>
{
    public async Task Handle(DepositWebhookCommand request, CancellationToken cancellationToken)
    {
        UserBalance userBalance = await transactionRepository.
            GetUserBalanceWithTransactionAsync(userId: request.UserId, reason: request.PaymentId.ToString(),
            reasonType: ReasonTypeEnum.NowPayment,
            cancellationToken)
            ?? throw new EntityNotFoundException<UserBalance>();

        userBalance.UpdateDepositStatus(
            Enum.Parse<DepositStatusEnum>(request.PaymentStatus, true),
            request.PaymentId,
            request.PayAddress);

        await unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
