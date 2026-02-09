using Investment.Domain.Common;
using Investment.Domain.Users.Entities;

namespace Investment.Domain.Users.Rules;

internal class RefreshTokenHashValidationRule(RefreshToken refreshToken, string refreshTokenHashString) : IBusinessRule
{
    public string Message => "RefreshToken isn't Exist!";

    public bool IsBroken() => !refreshToken.IsRefreshTokenHashValid(refreshTokenHashString);

    public BusinessRuleCodeEnum BusinessRuleCode => BusinessRuleCodeEnum.RefreshTokenHashValidation;

}