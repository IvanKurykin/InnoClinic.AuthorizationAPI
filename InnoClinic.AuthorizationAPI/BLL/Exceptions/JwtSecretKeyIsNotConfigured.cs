namespace BLL.Exceptions;

public class JwtSecretKeyIsNotConfigured : Exception
{
    private const string DefaultMessage = "JWT Secret Key is not configured.";
    public JwtSecretKeyIsNotConfigured() : base(DefaultMessage) { }
    public JwtSecretKeyIsNotConfigured(string message) : base(message) { }
    public JwtSecretKeyIsNotConfigured(string message, Exception innerException) : base(message, innerException) { }
}
