using Investment.Application.Utilities.Configs;
using Investment.Application.Utilities.Constants;
using Investment.Domain.Common;
using Investment.Domain.Users.Entities;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.Unicode;

namespace Investment.Infrastructure.Services;

public class TokenAuthenticateService(IOptions<AuthenticateConfig> authenticateConfigOptions) : ITokenAuthenticateService
{
    private readonly AuthenticateConfig authenticateConfig = authenticateConfigOptions.Value;

    public string GenerateJwtToken(string emailAddress, string userId, string refreshTokenId)
    {
        var secretKey = Encoding.UTF8.GetBytes(authenticateConfig.SecretKey);

        var symmetricSecurityKey = new SymmetricSecurityKey(secretKey);

        var signingCredentials = new SigningCredentials(symmetricSecurityKey,
            AuthenticateConstants.JwtSecurityAlgorithm);

        JwtSecurityToken token = new JwtSecurityToken(
            signingCredentials: signingCredentials,
            issuer: authenticateConfig.Issuer,
            audience: authenticateConfig.Audience,
            notBefore: DateTime.Now,
            expires: DateTime.Now.AddMinutes(authenticateConfig.JwtTokenExpiryTimeFrame.TotalMinutes),
            claims:
            [
              new Claim(ClaimTypes.Email, emailAddress),
              new Claim(AuthenticateConstants.UserIdClaimType,userId),
              new Claim(AuthenticateConstants.RefreshTokenIdClaimType, refreshTokenId),
            ]);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }


    public string GenerateRefreshToken()
    {
        var bytes = RandomNumberGenerator.GetBytes(64);

        return Convert.ToBase64String(bytes)
            .Replace("+", "-")
            .Replace("/", "_")
            .Replace("=", ""); // base64url-ish
    }

    public DateTime GetRefreshTokenExpireAt()
    {
        return DateTime.Now.AddDays(authenticateConfig.RefreshTokenExpireTimeToDay);
    }
}

