using System.Diagnostics.CodeAnalysis;

namespace BLL.Exceptions;

[ExcludeFromCodeCoverage]
public class JwtSecretKeyIsNotConfiguredException : Exception
{
    private const string DefaultMessage = "JWT Secret Key is not configured.";
    public JwtSecretKeyIsNotConfiguredException() : base(DefaultMessage) { }
    public JwtSecretKeyIsNotConfiguredException(string message) : base(message) { }
    public JwtSecretKeyIsNotConfiguredException(string message, Exception innerException) : base(message, innerException) { }
}
