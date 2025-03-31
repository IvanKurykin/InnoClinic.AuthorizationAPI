namespace BLL.Exceptions;

public class UserNameNullException : Exception
{
    private const string DefaultMessage = "User name cannot be null";
    public UserNameNullException() : base(DefaultMessage) { }
    public UserNameNullException(string message) : base(message) { }
    public UserNameNullException(string message, Exception innerException) : base(message, innerException) { }
}
