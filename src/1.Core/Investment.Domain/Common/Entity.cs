using Investment.Domain.Common.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Investment.Domain.Common;

public class Entity<TId> where TId : struct,
          IComparable,
          IComparable<TId>,
          IConvertible,
          IEquatable<TId>,
          IFormattable
{
    public TId Id { get; protected set; }
    protected void CheckRule(IBusinessRule rule)
    {
        if (rule.IsBroken())
        {
            throw new BusinessRuleValidationException(rule);
        }
    }

    protected static void CheckRuleStatic(IBusinessRule rule)
    {
        if (rule.IsBroken())
        {
            throw new BusinessRuleValidationException(rule);
        }
    }
}
