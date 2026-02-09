using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Investment.Domain.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Investment.Infrastructure.Domain.Common;

public class CommandRepository<TEntity, TId> :
    ICommandRepository<TEntity, TId> where TEntity :
    AggregateRoot<TId> where TId : struct, IComparable, IComparable<TId>, IConvertible, IEquatable<TId>, IFormattable

{
    private readonly InvestmentDbContext dbContext;

    private IDbContextTransaction _transaction=null!;

    public CommandRepository(InvestmentDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public async Task AddAsync(TEntity entity, CancellationToken cancellationToken)
    {
        await dbContext.Set<TEntity>().AddAsync(entity, cancellationToken);
    }

    public async Task<TEntity?> GetByIdAsync(TId id, CancellationToken cancellationToken)
    {
        return await dbContext.Set<TEntity>().FindAsync(id, cancellationToken);
    }

    public Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        return dbContext.SaveChangesAsync(cancellationToken);
    }

    public void BeginTransaction()
    {
        _transaction = dbContext.Database.BeginTransaction();
    }

    public void RollbackTransaction()
    {
        if (_transaction == null)
        {
            throw new NullReferenceException("Please call `BeginTransaction()` method first.");
        }
        _transaction.Rollback();
    }

    public void CommitTransaction()
    {
        if (_transaction == null)
        {
            throw new NullReferenceException("Please call `BeginTransaction()` method first.");
        }
        _transaction.Commit();
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
