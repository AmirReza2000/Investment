using Investment.Domain.Common;
using Investment.Domain.Users.Enums;

namespace Investment.Domain.Users.Entities
{
    internal class ValidVerifyTokenShouldNotExistRule : IBusinessRule
    {
        private User user;

        public ValidVerifyTokenShouldNotExistRule(User user)
        {
            this.user = user;
        }

        public BusinessRuleCodeEnum BusinessRuleCode => BusinessRuleCodeEnum.ValidVerifyTokenShouldNotExist;

        public string Message => "The previous code is still valid.";

        public bool IsBroken() => user.ValidationTokens.Any(
            validationToken =>
            validationToken.ValidationTokenType == ValidationTokenTypeEnum.UserActivation &&
            validationToken.ExpireAt > DateTime.Now &&
            validationToken.Revoked == false
            );

    }
}