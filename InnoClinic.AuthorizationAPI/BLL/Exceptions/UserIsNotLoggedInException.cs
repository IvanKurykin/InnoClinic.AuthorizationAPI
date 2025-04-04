using System.Diagnostics.CodeAnalysis;

namespace BLL.Exceptions;

[ExcludeFromCodeCoverage]
public class UserIsNotLoggedInException : Exception
{
    private const string DefaultMessage = "The user is not logged in. Try again.";
    public UserIsNotLoggedInException() : base(DefaultMessage) { }
    public UserIsNotLoggedInException(string message) : base(message) { }
    public UserIsNotLoggedInException(string message, Exception innerException) : base(message, innerException) { }
}
