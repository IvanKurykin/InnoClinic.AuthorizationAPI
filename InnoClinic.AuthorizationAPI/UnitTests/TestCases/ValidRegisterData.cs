namespace UnitTests.TestData;

using DAL.Constants;
using UnitTests;

public class ValidRegisterData : TheoryData<string, string, string, string>
{
    public ValidRegisterData()
    {
        Add(TestConstans.TestUserName, TestConstans.TestUserEmail, TestConstans.TestUserPassword, Roles.Admin);
        Add("anotherUser", "another@email.com", "P@ssw0rd123", Roles.Patient);
    }
}
