using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.DataProtection;

namespace Investment.Application.Utilities.Configs;

public class CryptoPaymentConfig
{
    public static string SectionNameInAppsettings => "CryptoPaymentConfig";

    public required string IpnSecretKey { get; set; }

    public required string ApiKey { get; set; }

    public required string BaseAddress { get; set; }

    public required bool IsSecure { get; set; }

    public required string EmailAddress { get; set; }

    public required string Password { get; set; }
}

