using System;
using System.Collections.Generic;
using System.Text;

namespace Investment.Domain.Common;

internal class GeneralBusinessRule : IBusinessRule
{
    public string Message { get; }

    public BusinessRuleCodeEnum BusinessRuleCode { get; }

    public GeneralBusinessRule(string message, BusinessRuleCodeEnum businessRuleCode)
    {
        Message = message;

        BusinessRuleCode = businessRuleCode;
    }

    public bool IsBroken()
    {
        return true;
    }

}
