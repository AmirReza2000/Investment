using System;
using System.Collections.Generic;
using System.Text;

namespace Investment.Domain.Common;

public class AggregateRoot<TId> : Entity<TId>, IAggregateRoot where TId : struct,
          IComparable,
          IComparable<TId>,
          IConvertible,
          IEquatable<TId>,
          IFormattable
{
}
