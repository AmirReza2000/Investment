using System;
using System.Collections.Generic;
using System.Text;
using Investment.Application.Utilities.Abstractions;
using Investment.Application.Utilities.Exceptions;
using Investment.Domain.Transactions.Entities;
using Investment.Domain.Transactions.Repositories;
using MediatR;

namespace Investment.Application.CryptoPayment.Commands.Withdrawal;

public class WithdrawalCommandHandler(
    IUserBalanceCommandRepository userBalanceRepository,
    IUnitOfWork unitOfWork,
    IUserInfoService userInfoService)
    : IRequestHandler<WithdrawalCommand, WithdrawalResponse>
{
    public async Task<WithdrawalResponse> Handle(WithdrawalCommand request, CancellationToken cancellationToken)
    {

        UserBalance userBalance = await userBalanceRepository
            .GetUserBalanceByUserIdAsync(userInfoService.GetUserId(), cancellationToken)
            ?? throw new EntityNotFoundException<UserBalance>();

        Transaction transaction =
            userBalance.CreateWithdrawal(
            amountWithdrawal: request.Amount,
            addressWithdrawal: request.AddressWithdrawal);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return new WithdrawalResponse { transactionId = transaction.Id };
    }
}
