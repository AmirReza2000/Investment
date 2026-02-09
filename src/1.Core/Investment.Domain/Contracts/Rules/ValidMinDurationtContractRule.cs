using System;
using System.Collections.Generic;
using System.Text;
using Investment.Domain.Common;

namespace Investment.Domain.Contracts.Rules;

internal class ValidMinDurationContractRule : IBusinessRule
{
    private short minDurationOfDay;
    private int durationOfDay;

    public ValidMinDurationContractRule(short minDurationOfDay, int durationOfDay)
    {
        this.minDurationOfDay = minDurationOfDay;
        this.durationOfDay = durationOfDay;
    }

    public string Message => $"minimum contract duration is {minDurationOfDay} days or {minDurationOfDay/30} months.";

    public BusinessRuleCodeEnum BusinessRuleCode =>
        BusinessRuleCodeEnum.ValidMinDurationContract;

    public bool IsBroken() => durationOfDay < minDurationOfDay;
}
