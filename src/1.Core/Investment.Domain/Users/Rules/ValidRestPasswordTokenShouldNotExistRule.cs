using Investment.Domain.Common;
using Investment.Domain.Users.Entities;
using Investment.Domain.Users.Enums;

namespace Investment.Domain.Users.Rules
{
    internal class ValidRestPasswordTokenShouldNotExistRule : IBusinessRule
    {
        private User user;

        public ValidRestPasswordTokenShouldNotExistRule(User user)
        {
            this.user = user;
        }

        public string Message => "The previous code is still valid.";

        public bool IsBroken() => user.ValidationTokens.Any(
            validationToken => 
            validationToken.ValidationTokenType==ValidationTokenTypeEnum.ResetPassword&&
            validationToken.ExpireAt > DateTime.Now&&
            validationToken.Revoked==false
            );

        public BusinessRuleCodeEnum BusinessRuleCode => BusinessRuleCodeEnum.ValidRestPasswordTokenShouldNotExist;

    }
}