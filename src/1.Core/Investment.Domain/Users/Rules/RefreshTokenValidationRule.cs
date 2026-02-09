using Investment.Domain.Common;
using Investment.Domain.Users.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Investment.Domain.Users.Rules;

public class RefreshTokenValidationRule : IBusinessRule
{
    private readonly RefreshToken _refreshToken;

    public RefreshTokenValidationRule(RefreshToken refreshToken)
    {
        _refreshToken = refreshToken;
    }

    public bool IsBroken() => (
               _refreshToken.Revoked || _refreshToken.ExpiresAt < DateTime.Now);

    public string Message => "RefreshToken isn't Exist!";

    public BusinessRuleCodeEnum BusinessRuleCode => BusinessRuleCodeEnum.RefreshTokenValidation;
}
