using System.Diagnostics.CodeAnalysis;

namespace BLL.Helpers;

[ExcludeFromCodeCoverage]
public static class ValidationPatterns
{
    public const string EmailRegex = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
}