using System.Security.Claims;

namespace Investment.Application.Utilities.DTO;

public sealed record JwtValidationResult(
bool IsValid,
ClaimsPrincipal? Principal,
string? Error,
Exception? Exception = null
);
