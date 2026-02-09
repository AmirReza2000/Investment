using System;
using System.Collections.Generic;
using System.Text;
using Investment.Application.Utilities.Abstractions;
using Investment.Domain.Transactions.Repositories;
using MediatR;

namespace Investment.Application.UserBalances.Commands.CreateUserBalance
{
    internal class CreateUserBalanceCommandHandler(
    IUserBalanceCommandRepository userBalanceRepository,
    IUnitOfWork unitOfWork,
    IUserInfoService userInfoService)
        : IRequestHandler<CreateUserBalanceCommand>
    {
        public async Task Handle(CreateUserBalanceCommand request, CancellationToken cancellationToken)
        {
            int userId = userInfoService.GetUserId();

            if (await userBalanceRepository.GetUserBalanceByUserIdAsync(userId,cancellationToken) is not null)
                return;

            await userBalanceRepository.CreateUserBalanceAsync(userId);

            await unitOfWork.SaveChangesAsync(cancellationToken);

        }
    }
}
