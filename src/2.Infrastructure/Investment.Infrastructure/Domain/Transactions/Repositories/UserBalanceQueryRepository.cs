using System;
using System.Collections.Generic;
using System.Text;
using Investment.Application.UserBalances.Queries.GetTransactions;
using Investment.Application.Utilities.PaginatedQuery;
using Investment.Application.Utilities.QueryRepositories;
using Investment.Domain.Transactions.Entities;
using Investment.Domain.Transactions.Enums.Transaction;
using Investment.Domain.Transactions.Repositories;
using Investment.Infrastructure.Domain.Common;
using Microsoft.EntityFrameworkCore;

namespace Investment.Infrastructure.Domain.Transactions.Repositories;

public class UserBalanceQueryRepository : QueryRepository,
    IUserBalanceQueryRepository
{
    private IQueryable<UserBalance> _userBalancesAsNoTracking => dbContext.UserBalances.AsNoTracking();

    public UserBalanceQueryRepository(InvestmentDbContext dbContext) : base(dbContext)
    {

    }

    public Task<UserBalance?> GetUserBalanceAsync(int userId, CancellationToken cancellationToken)
    {
        return _userBalancesAsNoTracking
            .Where(userbalance => userbalance.UserId == userId)
            .SingleOrDefaultAsync(cancellationToken);
    }

    public Task<TransactionResponse[]> GetUserBalanceTransactionsPaginatedAsync(
        GeUserBalanceTransactionsQuery request, int userId, CancellationToken cancellationToken)
    {

        var finalQuery = _userBalancesAsNoTracking
                 .Where(userBalance => userBalance.UserId == userId)
                 .SelectMany(balance => balance.Transactions);

        if (request.TransactionTypeFilter != TransactionTypeEnum.None)
        {
            if (!string.IsNullOrEmpty(request.TransactionStatusFilter))
            {
                finalQuery = finalQuery.Where(transaction =>
                       transaction.TransactionType == request.TransactionTypeFilter &&
                       transaction.TransactionStatus == request.TransactionStatusFilter);
            }
            else
            {
                finalQuery = finalQuery.Where(transaction =>
                   transaction.TransactionType == request.TransactionTypeFilter);
            }
        }
        if (request.FromDateTime.HasValue)
        {
            finalQuery = finalQuery.Where(transaction =>
             transaction.CreateDatetime >= request.FromDateTime);
        }

        if (request.ToDateTime.HasValue)
        {
            finalQuery = finalQuery.Where(transaction =>
                   transaction.CreateDatetime <= request.ToDateTime);
        }

        return finalQuery.ApplyPaging(request).Select(t => new TransactionResponse
        {
            Id = t.Id,
            Reason = t.Reason,
            ReasonType = t.ReasonType,
            TransactionStatus = t.TransactionStatus,
            TransactionType = t.TransactionType,
            Amount = t.Amount,
            Address = t.Address,
            CreateDatetime = t.CreateDatetime
        }).ToArrayAsync(cancellationToken: cancellationToken);
    }
}
