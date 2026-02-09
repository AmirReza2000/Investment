using System;
using System.Collections.Generic;
using System.Text;

namespace Investment.Domain.Common;

public interface IBusinessRule
{
    bool IsBroken();

    string Message { get; }

     BusinessRuleCodeEnum BusinessRuleCode { get; }
}