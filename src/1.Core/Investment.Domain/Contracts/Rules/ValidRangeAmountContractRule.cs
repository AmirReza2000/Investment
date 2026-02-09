using System;
using System.Collections.Generic;
using System.Text;
using Investment.Domain.Common;

namespace Investment.Domain.Contracts.Rules;

public class ValidRangeAmountContractRule : IBusinessRule
{
    private int amount;
    private int? maxAmount;
    private int minAmount;

    public ValidRangeAmountContractRule(decimal amount, int? maxAmount, int minAmount)
    {
        this.amount = ((int)amount);
        this.maxAmount = maxAmount;
        this.minAmount = minAmount;
    }

    public string Message => $"the amount contract should be between {minAmount}"+ (maxAmount is null ?"":$"and {maxAmount}") +" dollars.";


    public BusinessRuleCodeEnum BusinessRuleCode => BusinessRuleCodeEnum.ValidRangeAmountContract;

    public bool IsBroken()
    {
        if(maxAmount is not null )return maxAmount < amount || amount < minAmount;

        return amount < minAmount;
    }
}
