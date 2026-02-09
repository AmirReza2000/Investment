using System;
using System.Collections.Generic;
using System.Text;

namespace Investment.Application.Utilities.Configs;

public class EmailServerSettings
{
    public const string SectionNameInAppsettings = "EmailServerSettings";

    public required string Host { get; set; }
    public required string Username { get; set; }
    public required string Password { get; set; }
    public required string Sender { get; set; }
    public required int Port { get; set; }
    public required bool EnableSSl { get; set; }
}
