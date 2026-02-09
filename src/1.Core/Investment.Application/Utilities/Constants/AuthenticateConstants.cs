using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace Investment.Application.Utilities.Constants;

public static class AuthenticateConstants
{
    public const string RefreshTokenIdClaimType = ClaimTypes.PrimarySid;
    public const string UserIdClaimType = ClaimTypes.Sid;
    public const string JwtSecurityAlgorithm = SecurityAlgorithms.HmacSha512Signature;
    public const int EmailVerificationTokenLength=6;

}
