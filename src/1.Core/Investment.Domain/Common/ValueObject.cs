using Investment.Domain.Common.Exceptions;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Investment.Domain.Common;

public abstract class ValueObject : IEquatable<ValueObject>
{
    protected static void CheckRule(IBusinessRule rule)
    {
        if (rule.IsBroken())
        {
            throw new BusinessRuleValidationException(rule);
        }
    }

    public bool Equals(ValueObject? other)
    {
        throw new NotImplementedException();
    }
}