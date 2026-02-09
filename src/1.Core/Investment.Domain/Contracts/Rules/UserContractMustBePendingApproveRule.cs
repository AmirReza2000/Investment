using System;
using System.Collections.Generic;
using System.Text;
using Investment.Domain.Common;
using Investment.Domain.Contracts.Entities;

namespace Investment.Domain.Contracts.Rules;

public class UserContractMustBePendingApproveRule(UserContract userContract) : IBusinessRule
{
    public string Message => "Contract User Must Be in Pending Approve status.";

    public BusinessRuleCodeEnum BusinessRuleCode => BusinessRuleCodeEnum.UserContractMustBePendingApprove;

    public bool IsBroken() => userContract.Status != Enums.UserContractStatusEnum.PendingApprove;
}
