namespace UnitTests.TestCases;

using DAL.Entities;

public class UserEmailTestCases : TheoryData<string, User?>
{
    public UserEmailTestCases()
    {
        Add("nonexistent@example.com", null);
        Add("existing@example.com", new User { Email = "existing@example.com", UserName = TestConstans.TestUserName });
    }
}

