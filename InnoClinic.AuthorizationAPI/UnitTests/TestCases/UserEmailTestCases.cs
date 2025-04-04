using DAL.Entities;

namespace UnitTests.TestCases;

public class UserEmailTestCases : TheoryData<string, User>
{
    public UserEmailTestCases()
    {
        Add("test@example.com", new User { Email = "test@example.com", UserName = "TestUser" });
        Add("admin@example.com", new User { Email = "admin@example.com", UserName = "AdminUser" });
    }
}