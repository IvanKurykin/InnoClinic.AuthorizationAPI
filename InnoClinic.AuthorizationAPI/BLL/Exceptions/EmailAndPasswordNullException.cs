namespace BLL.Exceptions;

public class EmailAndPasswordNullException : Exception
{
    private const string DefaultMessage = "Email and Password cannot be null";
    public EmailAndPasswordNullException() : base(DefaultMessage) { }
    public EmailAndPasswordNullException(string message) : base(message) { }
    public EmailAndPasswordNullException(string message, Exception innerException) : base(message, innerException) { }
}
