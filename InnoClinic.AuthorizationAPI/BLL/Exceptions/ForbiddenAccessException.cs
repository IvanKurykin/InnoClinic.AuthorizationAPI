namespace BLL.Exceptions;

public class ForbiddenAccessException : Exception
{
    private const string DefaultMessage = "The user does not have access.";
    public ForbiddenAccessException() : base(DefaultMessage) { }
    public ForbiddenAccessException(string message) : base(message) { }
    public ForbiddenAccessException(string message, Exception innerException) : base(message, innerException) { }
}
