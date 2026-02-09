using Investment.Domain.Common;
using Investment.Domain.Users.Entities;
using Investment.Domain.Users.Enums.User;

namespace Investment.Domain.Users.Rules
{
    internal class UserMustBeVerifiedRule : IBusinessRule
    {
        private User user;

        public UserMustBeVerifiedRule(User user)
        {
            this.user = user;
        }

        public string Message => "user must be in verified status.";

        public bool IsBroken() => user.UserStatus == UserStatusEnum.NotVerified;

        public BusinessRuleCodeEnum BusinessRuleCode => BusinessRuleCodeEnum.UserMustBeVerified;

    }
}