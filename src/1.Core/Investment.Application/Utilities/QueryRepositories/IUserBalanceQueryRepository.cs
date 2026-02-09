using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Text;
using Investment.Application.UserBalances.Queries.GetTransactions;
using Investment.Domain.Transactions.Entities;

namespace Investment.Application.Utilities.QueryRepositories;

public interface IUserBalanceQueryRepository
{
    Task<UserBalance?> GetUserBalanceAsync(int userId, CancellationToken cancellationToken);
    Task<TransactionResponse[]> GetUserBalanceTransactionsPaginatedAsync(
        GeUserBalanceTransactionsQuery request, int userIdAsInt, CancellationToken cancellationToken);
}
