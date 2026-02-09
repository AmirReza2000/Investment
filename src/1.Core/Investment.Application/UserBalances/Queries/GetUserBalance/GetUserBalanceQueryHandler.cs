using System;
using System.Collections.Generic;
using System.Text;
using Investment.Application.Utilities.Abstractions;
using Investment.Application.Utilities.Exceptions;
using Investment.Application.Utilities.QueryRepositories;
using Investment.Domain.Transactions.Entities;
using Investment.Domain.Transactions.Repositories;
using MediatR;

namespace Investment.Application.UserBalances.Queries.GetUserBalance;

public class GetUserBalanceQueryHandler
    (IUserInfoService userInfoService,
    IUserBalanceQueryRepository userBalanceQueryRepository)
    : IRequestHandler<GetUserBalanceQuery, GetUserBalanceRepones>
{
    public async Task<GetUserBalanceRepones> Handle(GetUserBalanceQuery request, CancellationToken cancellationToken)
    {
        var userBalance = await userBalanceQueryRepository
                .GetUserBalanceAsync(userInfoService.GetUserId(), cancellationToken)
                ?? throw new EntityNotFoundException<UserBalance>();

        return new GetUserBalanceRepones(userBalance.Amount);

    }
}
