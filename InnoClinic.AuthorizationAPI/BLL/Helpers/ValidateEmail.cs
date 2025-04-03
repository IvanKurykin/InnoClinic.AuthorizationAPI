using System.Text.RegularExpressions;

namespace BLL.Helpers;

public static class ValidateEmail
{
    public static bool BeValidEmail(string? email)
    {
        if (string.IsNullOrWhiteSpace(email)) return false;

        return Regex.IsMatch(email,
            ValidationPatterns.EmailRegex,
            RegexOptions.IgnoreCase,
            TimeSpan.FromMilliseconds(250));
    }
}
