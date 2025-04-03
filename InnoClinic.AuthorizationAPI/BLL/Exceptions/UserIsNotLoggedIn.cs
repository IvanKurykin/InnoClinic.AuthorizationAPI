namespace BLL.Exceptions;

public class UserIsNotLoggedIn : Exception
{
    private const string DefaultMessage = "The user is not logged in. Try again.";
    public UserIsNotLoggedIn() : base(DefaultMessage) { }
    public UserIsNotLoggedIn(string message) : base(message) { }
    public UserIsNotLoggedIn(string message, Exception innerException) : base(message, innerException) { }
}
