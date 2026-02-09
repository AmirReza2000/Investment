using System;
using System.Collections.Generic;
using System.Text;
using Investment.Domain.Common;
using Investment.Domain.Contracts.Entities;

namespace Investment.Domain.Contracts.Rules;

public class UserContractMustBeApprovedRule(UserContract userContract) : IBusinessRule
{
    public string Message => "Contract User Must Be in Pending Approve status.";

    public BusinessRuleCodeEnum BusinessRuleCode => BusinessRuleCodeEnum.UserContractMustBeApproved;

    public bool IsBroken() => userContract.Status != Enums.UserContractStatusEnum.Approved;
}
