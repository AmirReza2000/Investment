using Investment.Application.Users.Notifications;
using Investment.Application.Utilities;
using Investment.Application.Utilities.Abstractions;
using Investment.Application.Utilities.Configs;
using Investment.Application.Utilities.Constants;
using Investment.Domain.Transactions.Entities;
using Investment.Domain.Transactions.Repositories;
using Investment.Domain.Users;
using Investment.Domain.Users.Entities;
using Investment.Domain.Users.Repositories;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace Investment.Application.Users.Commands;

internal class UserCreateHandler(
    IUserCommandRepository userCommandRepository,
    IUnitOfWork unitOfWork,
    IHashHelperService hashHelperService,
    IMediator mediator,
    IOptions<RegistrationVerifyEmailSettings> registrationSettingsOptions)
    : IRequestHandler<UserCreateCommand, int>
{
    private readonly RegistrationVerifyEmailSettings registrationSettings = registrationSettingsOptions.Value;

    public async Task<int> Handle(UserCreateCommand request, CancellationToken cancellationToken)
    {
        await userCommandRepository.AssertUserNotExistAsync(request.EmailAddress, cancellationToken);

        User user = User.Create(
            emailAddress: request.EmailAddress,
            password: request.Password,
            hashHelperService);

        var emailVerificationToken = RandomGenerator.GenerateValidationToken(
            AuthenticateConstants.EmailVerificationTokenLength);

        user.SetUserVerifyToken(
            token: emailVerificationToken,
            expireAt: DateTime.Now.AddMinutes(registrationSettings.TokenExpireToMinute),
            hashHelperService);

        await userCommandRepository.AddAsync(user, cancellationToken);

        await mediator.Publish(new UserRegisteredNotification(
                   emailAddress: request.EmailAddress,
                   validationToken: emailVerificationToken),
                       cancellationToken);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return user.Id;
    }
}
