using Microsoft.AspNetCore.Identity;
using UnitTests;

public class LoginTestCases : TheoryData<string, string, bool, SignInResult, bool>
{
    public LoginTestCases()
    {
        Add(TestConstans.TestUserName, TestConstans.TestUserPassword, false, SignInResult.Success, true);
        Add("invalid", "invalid", false, SignInResult.Failed, false);
        Add("locked", "user", true, SignInResult.LockedOut, false);
    }
}