using Investment.Application.Utilities.Abstractions;
using Investment.Domain.Common;
using Investment.Domain.Common.Exceptions;
using Investment.Domain.Users;
using Investment.Domain.Users.Entities;
using Investment.Domain.Users.Repositories;
using Investment.Domain.Users.Rules;
using MediatR;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace Investment.Application.Authenticate.Login;

public class LoginCommandHandler(
    IUserCommandRepository userCommandRepository,
    IUnitOfWork unitOfWork,
    ITokenAuthenticateService tokenAuthenticateService,
    IHashHelperService hashHelperService) :
    IRequestHandler<LoginCommand, LoginResponse>
{
    public async Task<LoginResponse> Handle(LoginCommand request, 
        CancellationToken cancellationToken)
    {
        var user = await userCommandRepository.
            GetUserByEmailWithValidRefreshTokensAsync(emailAddress: request.EmailAddress, cancellationToken);

        if (user == null) throw new BusinessRuleValidationException(
            new ValidUsernameAndPasswordRule(null!, null!));

        user.Login(password: request.Password,hashHelperService);

        var refreshTokenString = tokenAuthenticateService.GenerateRefreshToken();

        var refreshTokenNew = user.SetRefreshToken(refreshToken: refreshTokenString,
            refreshTokenExpiresAt: tokenAuthenticateService.GetRefreshTokenExpireAt(),
             deviceName: request.deviceName,
             userAgent: request.userAgent,
             ipAddress: request.ipAddress,
             hashHelperService
            );

        await unitOfWork.SaveChangesAsync(cancellationToken);

        var jwtToken = tokenAuthenticateService.GenerateJwtToken(
            emailAddress: user.EmailAddress,
            userId: user.Id.ToString(),
            refreshTokenId: refreshTokenNew.Id.ToString());

        return new LoginResponse
        {
            AccessKey = jwtToken,
            RefreshToken = refreshTokenString,
        };
    }
}
