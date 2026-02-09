using Investment.Application.Utilities.Abstractions;
using Investment.Application.Utilities.Constants;
using Investment.Domain.Common;
using Investment.Domain.Users;
using Investment.Domain.Users.Repositories;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Text;

namespace Investment.Application.Authenticate.RefreshToken;

internal class RefreshTokenHandler(
     ITokenAuthenticateService tokenAuthenticateService,
    IUserCommandRepository userCommandRepository,
    IUnitOfWork unitOfWork,
    IHttpContextAccessor httpContextAccessor,
    IHashHelperService hashHelperService,
    IJwtTokenService jwtTokenService
    )
    : IRequestHandler<RefreshTokenCommand, RefreshTokenResponse?>
{
    public async Task<RefreshTokenResponse?> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
    {

        var HasTokenBeenSent = httpContextAccessor.HttpContext!.Request.Headers.TryGetValue("Authorization", out var token);

        if (!HasTokenBeenSent)
            return null;

        var jwtValidationResult = jwtTokenService.ValidateToken(token.ToString(), ignoreLifetime: true);

        if (!jwtValidationResult.IsValid)
            return null;

        string? userId =
           jwtTokenService.GetClaim(jwtValidationResult.Principal!, AuthenticateConstants.UserIdClaimType);

        string? refreshTokenId =
         jwtTokenService.GetClaim(jwtValidationResult.Principal!, AuthenticateConstants.RefreshTokenIdClaimType);

        if (userId is null || refreshTokenId is null) return null;

        var user = await userCommandRepository.GetUserForRotateRefreshTokensAsync(
                                                userId: int.Parse(userId),
                                                refreshTokenId: int.Parse(refreshTokenId),
                                                cancellationToken);

        if (user is null) return null;

        var newRefreshTokenString = tokenAuthenticateService.GenerateRefreshToken();

        var newRefreshTokenId = user!.RotateRefreshToken(int.Parse(refreshTokenId),
            request.RefreshToken, newRefreshTokenString, hashHelperService);

        if (newRefreshTokenId is null) return null;

        await unitOfWork.SaveChangesAsync(cancellationToken);

        var newAccessKey = tokenAuthenticateService.GenerateJwtToken(user.EmailAddress, userId, newRefreshTokenId.ToString()!);

        return new RefreshTokenResponse(
                AccessKey: newAccessKey,
                RefreshToken: newRefreshTokenString);
    }
}
