﻿using System.Diagnostics.CodeAnalysis;

namespace BLL.Helpers.Settings;

[ExcludeFromCodeCoverage]
public class JwtSettings
{
    public string? SecretKey { get; set; }
    public string? Issuer { get; set; }
    public string? Audience { get; set; }
    public int ExpiryInHours { get; set; }
}
