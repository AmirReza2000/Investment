using System;
using System.Collections.Generic;
using System.Text;
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

namespace Investment.Application.Authenticate.ResetPassword;

public class SendEmailResetPasswordCommandHandler (
    IUserCommandRepository userCommandRepository,
    IHashHelperService hashHelperService,
    IEmailSenderService emailSender,
    IUnitOfWork unitOfWork,
    IOptions<ResetPasswordVerifyEmailSettings> ResetPasswordVerifyEmailSettingsOption
    )
    : IRequestHandler<SendEmailResetPasswordCommand>
{

    private readonly ResetPasswordVerifyEmailSettings resetPasswordVerifyEmailSettings = ResetPasswordVerifyEmailSettingsOption.Value;

    public async Task Handle(SendEmailResetPasswordCommand request, CancellationToken cancellationToken)
    {
        var user=await userCommandRepository.
            GetUserWithRestPasswordValidTokensAsync(request.EmailAddress, cancellationToken);

        if (user is null)
            throw new EntityNotFoundException<User>();

        var token = 
            RandomGenerator.GenerateValidationToken(AuthenticateConstants.EmailVerificationTokenLength);

        var tokenExpire =
            DateTime.Now.AddMinutes(resetPasswordVerifyEmailSettings.TokenExpireToMinute);

        user.SetRestPasswordToken(token, tokenExpire, hashHelperService);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        var replaceableTexts = new Dictionary<string, string>
        {
            { "{{CODE}}", token }
        };

        await emailSender.SendEmail(
         toEmailAddress: request.EmailAddress,
         verifyToken: token,
         bucketNameTemplate: resetPasswordVerifyEmailSettings.BucketName,
         objectNameInS3Template: resetPasswordVerifyEmailSettings.ObjectNameInS3,
         titleEmail: resetPasswordVerifyEmailSettings.Title,
         replaceableTexts: replaceableTexts,
         cancellationToken);
    }
}
