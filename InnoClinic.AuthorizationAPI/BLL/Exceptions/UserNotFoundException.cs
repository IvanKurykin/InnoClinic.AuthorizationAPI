namespace BLL.Exceptions;

public class UserNotFoundException : Exception
{
    private const string DefaultMessage = "The user was not found";
    public UserNotFoundException() : base(DefaultMessage) { }
    public UserNotFoundException(string message) : base(message) { }
    public UserNotFoundException(string message, Exception innerException) : base(message, innerException) { }
}
