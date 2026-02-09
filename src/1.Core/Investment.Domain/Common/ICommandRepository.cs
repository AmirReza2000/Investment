using System;
using System.Collections.Generic;
using System.Text;

namespace Investment.Domain.Common;

public interface ICommandRepository<TEntity, TId>  where TEntity : AggregateRoot<TId> where TId : struct, IComparable, IComparable<TId>, IConvertible, IEquatable<TId>, IFormattable
{
    Task AddAsync(TEntity entity, CancellationToken cancellationToken);
    Task<TEntity?> GetByIdAsync(TId id, CancellationToken cancellationToken);
}
