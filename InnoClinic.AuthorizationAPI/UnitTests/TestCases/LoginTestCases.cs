namespace UnitTests.TestCaces;

using Microsoft.AspNetCore.Identity;

public class LoginTestCases : TheoryData<string, string, bool, SignInResult, bool>
{
    public LoginTestCases()
    {
        Add("user1", "pass1", false, SignInResult.Success, true);
        Add("user2", "pass2", true, SignInResult.Failed, false);
        Add("user3", "pass3", false, SignInResult.LockedOut, false);
    }
}
