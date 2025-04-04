using System.Diagnostics.CodeAnalysis;

namespace API;

[ExcludeFromCodeCoverage]
public static class Messages
{
    public const string UserIsNotLoggedIn = "Your credentials are incorrect. Please try again";
    public const string UserLoggedOutSuccessfully = "User logged out successfully";
    public const string UserLoggedInSuccessfully = "User logged in successfully";
}
