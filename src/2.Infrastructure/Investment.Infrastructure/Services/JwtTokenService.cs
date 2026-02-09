using Investment.Application.Utilities.Abstractions;
using Investment.Application.Utilities.Configs;
using Investment.Application.Utilities.Constants;
using Investment.Application.Utilities.DTO;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Investment.Infrastructure.Services;

public sealed class JwtTokenService : IJwtTokenService
{
    private readonly AuthenticateConfig _authenticateConfig;
    private readonly byte[] _keyBytes;
    private readonly JwtSecurityTokenHandler _handler = new();

    public JwtTokenService(IOptions<AuthenticateConfig> opts)
    {
        _authenticateConfig = opts.Value;
        _keyBytes = Encoding.UTF8.GetBytes(_authenticateConfig.SecretKey);
    }

    public JwtValidationResult ValidateToken(string token, bool ignoreLifetime = false)
    {
        if (string.IsNullOrWhiteSpace(token))
            return new JwtValidationResult(false, null, "Token is empty.");

        if (token.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
            token = token["Bearer ".Length..].Trim();

        var parameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = _authenticateConfig.Issuer,

            ValidateAudience = true,
            ValidAudience = _authenticateConfig.Audience,

            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(_keyBytes),

            ValidateLifetime = !ignoreLifetime,
            ClockSkew = TimeSpan.Zero,

            RequireExpirationTime = true
        };
        try
        {
            var principal = _handler.ValidateToken(token, parameters, out var validatedToken);

            if (validatedToken is not JwtSecurityToken jwt ||
                !jwt.Header.Alg.Equals(AuthenticateConstants.JwtSecurityAlgorithm, StringComparison.OrdinalIgnoreCase))
            {
                return new JwtValidationResult(false, null, "Invalid token algorithm.");
            }

            return new JwtValidationResult(true, principal, null);
        }
        catch (SecurityTokenExpiredException ex)
        {
            return new JwtValidationResult(false, null, "Token expired.", ex);
        }
        catch (SecurityTokenException ex)
        {
            return new JwtValidationResult(false, null, $"Token invalid: {ex.Message}", ex);
        }
        catch (Exception ex)
        {
            return new JwtValidationResult(false, null, $"Validation failed: {ex.Message}", ex);
        }
    }

    public string? GetClaim(ClaimsPrincipal principal, string claimType)
        => principal.FindFirst(claimType)?.Value;

    public T? GetClaimAs<T>(
        ClaimsPrincipal principal,
        string claimType,
        Func<string, T> converter
    )
    {
        var value = GetClaim(principal, claimType);
        if (string.IsNullOrWhiteSpace(value)) return default;

        try { return converter(value); }
        catch { return default; }
    }
}
