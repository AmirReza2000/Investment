using Investment.Application.Utilities.Abstractions;
using Investment.Application.Utilities.Constants;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace Investment.Infrastructure.Services;

public class UserInfoService(IHttpContextAccessor httpContext) : IUserInfoService
{

    private string GetClaim(string claimType)
    {
        return httpContext.HttpContext?.User.FindFirst(claimType)?.Value
            ?? throw new NullReferenceException($"claimType:{claimType} isn't exist in token.");
    }

    public int GetUserId()
    {
        
        return int.Parse(GetClaim(AuthenticateConstants.UserIdClaimType));
    }

    public int GetRefreshTokenId()
    {
        return int.Parse(GetClaim(AuthenticateConstants.RefreshTokenIdClaimType));
    }
}
