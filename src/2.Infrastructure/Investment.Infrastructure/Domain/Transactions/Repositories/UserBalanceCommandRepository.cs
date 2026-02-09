using System;
using System.Collections.Generic;
using System.Text;
using Investment.Application.Utilities.Exceptions;
using Investment.Domain.Transactions.Entities;
using Investment.Domain.Transactions.Enums.Transaction;
using Investment.Domain.Transactions.Repositories;
using Investment.Domain.Users.Entities;
using Investment.Domain.Users.Repositories;
using Investment.Infrastructure.Domain.Common;
using Microsoft.EntityFrameworkCore;

namespace Investment.Infrastructure.Domain.Transactions.Repositories;

public class UserBalanceCommandRepository :
     CommandRepository<UserBalance, int>,
    IUserBalanceCommandRepository
{
    private readonly InvestmentDbContext _dbContext;

    public UserBalanceCommandRepository(InvestmentDbContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
    }

    public Task<UserBalance?> GetUserBalanceWithTransactionAsync(int userId, string reason, ReasonTypeEnum reasonType, CancellationToken cancellationToken)
    {
        return _dbContext.UserBalances
            .Include(userBalance =>
                  userBalance.Transactions
                  .Where(transaction =>
                  transaction.Reason == reason
                  && transaction.ReasonType == reasonType))
           .Where(userBalance =>
                userBalance.UserId == userId)
           .SingleOrDefaultAsync(cancellationToken);
    }

    public  Task<UserBalance?> GetUserBalanceByUserIdAsync(int userId,CancellationToken cancellationToken)
    {
        return  _dbContext.UserBalances
            .Where(userBalance => userBalance.UserId == userId)
            .SingleOrDefaultAsync(cancellationToken);       
    }

    public async Task CreateUserBalanceAsync(int userId)
    {

        UserBalance newUserBalance = UserBalance.Create(userId);

        await AddAsync(newUserBalance, default);

    }

    public Task<UserBalance?> GetByUserIdWithSpecificTransactionAsync(int userId, int transactionId, CancellationToken cancellationToken)
    {
        return _dbContext.UserBalances
                    .Include(userBalance =>
                          userBalance.Transactions
                          .Where(transaction =>
                          transaction.Id == transactionId))
                   .Where(userBalance =>
                        userBalance.UserId == userId)
                   .FirstOrDefaultAsync(cancellationToken);
    }

}
