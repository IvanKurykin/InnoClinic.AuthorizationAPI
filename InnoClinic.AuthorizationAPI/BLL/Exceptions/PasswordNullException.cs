namespace BLL.Exceptions;

public class PasswordNullException : Exception
{
    private const string DefaultMessage = "Password cannot be null";
    public PasswordNullException() : base(DefaultMessage) { }
    public PasswordNullException(string message) : base(message) { }
    public PasswordNullException(string message, Exception innerException) : base(message, innerException) { }
}
