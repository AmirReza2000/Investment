using System;
using System.Collections.Generic;
using System.Text;
using Investment.Application.Users.Notifications;
using Investment.Application.Utilities;
using Investment.Application.Utilities.Abstractions;
using Investment.Application.Utilities.Configs;
using Investment.Application.Utilities.Constants;
using Investment.Application.Utilities.Exceptions;
using Investment.Domain.Users;
using Investment.Domain.Users.Entities;
using Investment.Domain.Users.Repositories;
using MediatR;
using Microsoft.Extensions.Options;

namespace Investment.Application.Authenticate.ResendUserVerifyToken;

public class ResendUserVerifyTokenCommandHandler(
   IUserCommandRepository userCommandRepository,
   IUnitOfWork unitOfWork,
    IHashHelperService hashHelperService,
    IMediator mediator,
    IOptions<RegistrationVerifyEmailSettings> registrationSettingsOptions)
    : IRequestHandler<ResendUserVerifyTokenCommand>
{

    private readonly RegistrationVerifyEmailSettings registrationSettings = registrationSettingsOptions.Value;

    public async Task Handle(ResendUserVerifyTokenCommand request, CancellationToken cancellationToken)
    {
        var user = await userCommandRepository.
            GetUserWithVerifyValidTokensAsync(request.EmailAddress, cancellationToken) ?? throw new EntityNotFoundException<User>();

        var emailVerificationToken = RandomGenerator.GenerateValidationToken(
            AuthenticateConstants.EmailVerificationTokenLength);

        user.SetUserVerifyToken(
            token: emailVerificationToken,
            expireAt: DateTime.Now.AddMinutes(registrationSettings.TokenExpireToMinute),
            hashHelperService);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        await mediator.Publish(new UserRegisteredNotification(
                   emailAddress: request.EmailAddress,
                   validationToken: emailVerificationToken),
                   cancellationToken);

    }
}
