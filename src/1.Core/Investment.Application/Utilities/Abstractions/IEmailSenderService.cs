using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Win32;

namespace Investment.Application.Utilities.Abstractions;

public interface IEmailSenderService
{
    Task SendEmail(string toEmailAddress,
          string verifyToken,
          string bucketNameTemplate,
          string objectNameInS3Template,
          string titleEmail,
          Dictionary<string, string> replaceableTexts,
          CancellationToken cancellationToken);
}
