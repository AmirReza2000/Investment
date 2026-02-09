using System.Net;
using System.Net.Mail;
using Investment.Application.Utilities.Abstractions;
using Investment.Application.Utilities.Configs;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Investment.Infrastructure.Services;

public class EmailSenderService : IEmailSenderService
{
    private readonly EmailServerSettings _emailServerSettings;

    private readonly RegistrationVerifyEmailSettings _registrationVerifyEmailSettings;

    private readonly ILogger<EmailSenderService> _logger;

    private readonly IObjectStoreService _objectStoreService;

    public EmailSenderService(IOptions<EmailServerSettings> emailSettingsOptions,
        ILogger<EmailSenderService> logger, IOptions<RegistrationVerifyEmailSettings> registrationEmailOptions, IObjectStoreService objectStoreService)
    {
        _emailServerSettings = emailSettingsOptions.Value;
        _registrationVerifyEmailSettings = registrationEmailOptions.Value;
        _logger = logger;
        _objectStoreService = objectStoreService;
    }
    public async Task SendEmail(string toEmailAddress,
        string verifyToken,
        string bucketNameTemplate,
        string objectNameInS3Template,
        string titleEmail,
        Dictionary<string, string> replaceableTexts,
        CancellationToken cancellationToken)
    {
        try
        {
            using SmtpClient client = CreateSmtpClient();

            (MemoryStream stream, string _) = await _objectStoreService.DownloadFileAsync(
               bucketName: bucketNameTemplate,
               objectName: objectNameInS3Template
               );

            using var streamReader = new StreamReader(stream);

            var templateText = streamReader.ReadToEnd();
            string htmlBodyFormatted= templateText;
            foreach (var text in replaceableTexts)
            {
                htmlBodyFormatted= htmlBodyFormatted.Replace(text.Key, text.Value);
            }

            var message = new MailMessage(_emailServerSettings.Sender,
               toEmailAddress,
              titleEmail,
               htmlBodyFormatted);

            message.IsBodyHtml = true;

            await client.SendMailAsync(message, cancellationToken);
        }


        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to send register verification email to {ToEmailAddress}", toEmailAddress);
        }
    }

    private SmtpClient CreateSmtpClient()
    {
        return new SmtpClient(_emailServerSettings.Host)
        {
            Port = _emailServerSettings.Port,
            Credentials = new NetworkCredential(_emailServerSettings.Username, _emailServerSettings.Password),
            EnableSsl = _emailServerSettings.EnableSSl
        };
    }

}
