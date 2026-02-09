using Investment.Application.Utilities.Abstractions;
using Investment.Application.Utilities.Exceptions;
using Investment.Application.Utilities.QueryRepositories;
using Investment.Domain.Contracts.Entities;
using Investment.Domain.Contracts.Enums;
using Investment.Domain.Transactions.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Investment.Application.Users.Queries.Dashboard;

public class DashboardQueryHandler
    (IUserBalanceQueryRepository userBalanceQueryRepository,
    IContractQueryRepository contractQueryRepository,
    IUserInfoService userInfoService)
    : IRequestHandler<DashboardQuery, DashboardResponse>
{
    public async Task<DashboardResponse> Handle(DashboardQuery request, CancellationToken cancellationToken)
    {
        int userId = userInfoService.GetUserId();

        var userBalance = await userBalanceQueryRepository.GetUserBalanceAsync(userId: userId, cancellationToken)
            ?? throw new EntityNotFoundException<UserBalance>();

        UserContract[] userContracts = await contractQueryRepository.GetUserContractsWithProfitsAsync(userId: userId, cancellationToken);

        int countUserContracts = userContracts.Count();

        int activeContracts = userContracts.Count(userContract => userContract.Status == UserContractStatusEnum.Approved);

        decimal totalProfit = 0m;

        decimal lockedInContract = 0m;

        decimal futureProfit = 0m;

        decimal totalBalance = userBalance.Amount;

        foreach (UserContract userContract in userContracts)
        {
            totalProfit += userContract.CalculateTotalProfitExpected();

            totalBalance += userContract.Amount;

            lockedInContract += userContract.CalculateLockedAmount();

            futureProfit += userContract.CalculateUnpaidProfits();
        }

        totalBalance += futureProfit;

        return new DashboardResponse(
            FreeAmount: userBalance.Amount,
            TotalBalance: totalBalance,
            CountContracts: countUserContracts,
            ActiveContracts: activeContracts,
            TotalProfit: totalProfit,
            LockedInContract: lockedInContract,
            FutureProfit: futureProfit);
    }
}
