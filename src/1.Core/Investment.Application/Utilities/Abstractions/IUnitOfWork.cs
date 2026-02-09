using System;
using System.Collections.Generic;
using System.Text;

namespace Investment.Application.Utilities.Abstractions;

public interface IUnitOfWork
{
    Task ExecuteInTransactionAsync(Func<Task> action);

    void BeginTransaction();

    void CommitTransaction();

    void RollbackTransaction();

    Task SaveChangesAsync(CancellationToken cancellationToken);
}
