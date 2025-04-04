namespace UnitTests.TestCaces;

using DAL.Constants;
using UnitTests;

public class RegisterWithEmptyValuesData : TheoryData<string?, string?>
{
    public RegisterWithEmptyValuesData()
    {
        Add(null, null);
        Add(null, Roles.Admin);
        Add(TestConstans.TestUserPassword, null);
    }
}