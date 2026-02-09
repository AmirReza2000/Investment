using Investment.Domain.Common;
using Investment.Domain.Users.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Investment.Domain.Users.Rules;

public class ValidUsernameAndPasswordRule : IBusinessRule
{
    private readonly User _user;
    private readonly string _hashedPassword;
    public ValidUsernameAndPasswordRule(User user, string hashedPassword)
    {
        _user = user;
        _hashedPassword = hashedPassword;
    }

    public bool IsBroken() => _user.HashedPassword != _hashedPassword;

    public string Message => "Invalid username or password.";

    public BusinessRuleCodeEnum BusinessRuleCode => BusinessRuleCodeEnum.ValidUsernameAndPassword;

}
