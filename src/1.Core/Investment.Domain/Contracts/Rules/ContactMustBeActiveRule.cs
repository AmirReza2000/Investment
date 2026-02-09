using System;
using System.Collections.Generic;
using System.Text;
using Investment.Domain.Common;
using Investment.Domain.Contracts.Entities;

namespace Investment.Domain.Contracts.Rules;

public class ContactMustBeActiveRule : IBusinessRule
{
    private Contract _contract;

    public ContactMustBeActiveRule(Contract contract)
    {
        _contract = contract;
    }

    public string Message => "contract is not active.";

    public BusinessRuleCodeEnum BusinessRuleCode => BusinessRuleCodeEnum.ContractMustBeActive;

    public bool IsBroken() => !_contract.IsActive;
}
