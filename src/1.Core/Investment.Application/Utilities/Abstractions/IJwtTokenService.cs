using Investment.Application.Utilities.DTO;
using System.Security.Claims;

namespace Investment.Application.Utilities.Abstractions;

public interface IJwtTokenService
{
    JwtValidationResult ValidateToken(string token, bool ignoreLifetime = false);

    string? GetClaim(ClaimsPrincipal principal, string claimType);

    T? GetClaimAs<T>(ClaimsPrincipal principal, string claimType, Func<string, T> converter);
}
