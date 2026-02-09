using System;
using System.Collections.Generic;
using System.Text;

namespace Investment.Application.Utilities.Configs;

public class ObjectStoreConfig
{
    public const string SectionNameInAppsettings = "ObjectStoreConfig";

    public required bool IsSecure { get; set; }
    public required string AccessKey { get; set; }
    public required string SecretAccessKey { get; set; }
    public required string Hostname { get; set; }
}
