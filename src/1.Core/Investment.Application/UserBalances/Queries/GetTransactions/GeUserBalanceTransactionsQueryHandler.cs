using System;
using System.Collections.Generic;
using System.Text;
using Investment.Application.Utilities.Abstractions;
using Investment.Application.Utilities.QueryRepositories;
using Investment.Domain.Transactions.Repositories;
using MediatR;

namespace Investment.Application.UserBalances.Queries.GetTransactions;

internal class GeUserBalanceTransactionsQueryHandler(
    IUserBalanceQueryRepository userBalanceQueryRepository,
    IUserInfoService userInfoService)
    : IRequestHandler<GeUserBalanceTransactionsQuery, TransactionResponse[]>
{
    public async Task<TransactionResponse[]> Handle(GeUserBalanceTransactionsQuery request, CancellationToken cancellationToken)
    {
        return await userBalanceQueryRepository.GetUserBalanceTransactionsPaginatedAsync(
             request,
             userInfoService.GetUserId(),
             cancellationToken);
    }
}
