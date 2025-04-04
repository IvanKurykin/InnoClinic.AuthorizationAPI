using DAL.Constants;
using UnitTests;

public class LoginWithEmptyValuesData : TheoryData<string?, string?>
{
    public LoginWithEmptyValuesData()
    {
        Add(null, null);
        Add(null, Roles.Admin);
        Add(TestConstans.TestUserEmail, null);
    }
}
