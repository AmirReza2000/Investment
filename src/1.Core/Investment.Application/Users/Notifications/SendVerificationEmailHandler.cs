using System;
using System.Collections.Generic;
using System.Text;
using Investment.Application.Utilities.Abstractions;
using Investment.Application.Utilities.Configs;
using Investment.Domain.Users.Repositories;
using MediatR;
using Microsoft.Extensions.Options;

namespace Investment.Application.Users.Notifications;

public class SendVerificationEmailHandler(
      IEmailSenderService emailSender,
    IOptions<RegistrationVerifyEmailSettings> registrationSettingsOptions)
    : INotificationHandler<UserRegisteredNotification>
{
    private readonly RegistrationVerifyEmailSettings registrationSettings = registrationSettingsOptions.Value;
    
    public async Task Handle(UserRegisteredNotification notification, CancellationToken cancellationToken)
    {
        var replaceableTexts = new Dictionary<string, string>
        {
            { "{{CODE}}", notification.validationToken }
        };

        await emailSender.SendEmail(
          toEmailAddress:  notification.emailAddress,
          verifyToken:notification.validationToken,
          bucketNameTemplate: registrationSettings.BucketName,
          objectNameInS3Template: registrationSettings.ObjectNameInS3,
          titleEmail:registrationSettings.Title,
          replaceableTexts: replaceableTexts,
          cancellationToken);
    }
}
