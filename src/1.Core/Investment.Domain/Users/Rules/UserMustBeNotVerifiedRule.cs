using Investment.Domain.Common;
using Investment.Domain.Users.Entities;
using Investment.Domain.Users.Enums.User;

namespace Investment.Domain.Users.Rules;

internal class UserMustBeNotVerifiedRule : IBusinessRule
{
    private readonly User _user;

    public UserMustBeNotVerifiedRule(User user)
    {
        this._user = user;
    }

    public string Message => "The user has already been verified.";

    public bool IsBroken() => _user.UserStatus != UserStatusEnum.NotVerified;

    public BusinessRuleCodeEnum BusinessRuleCode => BusinessRuleCodeEnum.UserMustBeNotVerified;

}