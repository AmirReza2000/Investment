using System;
using System.Collections.Generic;
using System.Text;

namespace Investment.Application.Utilities.Configs;

public class AuthenticateConfig
{
    public const string SectionNameInAppsettings = "AuthenticateConfig";

    public required string Issuer { get; set; }

    public required string Audience { get; set; }

    public required TimeSpan JwtTokenExpiryTimeFrame { get; set; }

    public required int RefreshTokenExpireTimeToDay { get; set; }

    public required string SecretKey { get; set; }
}
