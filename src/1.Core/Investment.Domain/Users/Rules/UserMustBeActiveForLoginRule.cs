using Investment.Domain.Common;
using Investment.Domain.Users.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Investment.Domain.Users.Rules;

internal class UserMustBeActiveForLoginRule : IBusinessRule
{
    private readonly User _user;

    public UserMustBeActiveForLoginRule(User user)
    {
        _user = user;
    }

    public bool IsBroken() => !_user.IsActive();

    public string Message => $"For Login User Must Active.User Status:{_user.UserStatus}";

    public BusinessRuleCodeEnum BusinessRuleCode => BusinessRuleCodeEnum.UserMustBeActiveForLogin;

}
