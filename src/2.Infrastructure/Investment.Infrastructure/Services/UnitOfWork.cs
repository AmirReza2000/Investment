using System;
using System.Collections.Generic;
using System.Text;
using Investment.Application.Utilities.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Investment.Infrastructure.Services;

public class UnitOfWork(InvestmentDbContext dbContext) : IUnitOfWork
{
    private  IDbContextTransaction transaction=null!;

    public Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        return dbContext.SaveChangesAsync(cancellationToken);
    }

    public void BeginTransaction()
    {
        transaction = dbContext.Database.BeginTransaction();
    }

    public void RollbackTransaction()
    {
        if (transaction == null)
        {
            throw new NullReferenceException("Please call `BeginTransaction()` method first.");
        }
        transaction.Rollback();
    }

    public void CommitTransaction()
    {
        if (transaction == null)
        {
            throw new NullReferenceException("Please call `BeginTransaction()` method first.");
        }
        transaction.Commit();
    }

    public async Task ExecuteInTransactionAsync(Func<Task> action)
    {

        await using var transaction = await dbContext.Database.BeginTransactionAsync();
        try
        {
            await action();
            await transaction.CommitAsync();
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }
}
