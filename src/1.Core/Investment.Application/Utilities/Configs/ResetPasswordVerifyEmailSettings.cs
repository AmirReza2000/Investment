using System;
using System.Collections.Generic;
using System.Text;

namespace Investment.Application.Utilities.Configs;

public class ResetPasswordVerifyEmailSettings
{
    public const string SectionNameInAppsettings = "ResetPasswordVerifyEmailSettings";

    public required string Title { get; set; }

    public required string ObjectNameInS3 { get; set; }

    public required string BucketName { get; set; }

    public required int TokenExpireToMinute { get; set; }
}
