using Investment.Domain.Common;
using Investment.Domain.Transactions.Entities;
using Investment.Domain.Transactions.Enums.Transaction;
using Investment.Domain.Users.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Investment.Domain.Transactions.Repositories;

public interface IUserBalanceCommandRepository : ICommandRepository<UserBalance, int>
{
    Task<UserBalance?> GetUserBalanceWithTransactionAsync(int userId, string reason, ReasonTypeEnum reasonType, CancellationToken cancellationToken);
    Task<UserBalance?> GetUserBalanceByUserIdAsync(int userId,CancellationToken cancellationToken);
    Task<UserBalance?> GetByUserIdWithSpecificTransactionAsync(int userId, int transactionId, CancellationToken cancellationToken);
    Task CreateUserBalanceAsync(int userId);
}
