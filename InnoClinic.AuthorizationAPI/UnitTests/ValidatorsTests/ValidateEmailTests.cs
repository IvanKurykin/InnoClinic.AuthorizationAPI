using BLL.Helpers;

namespace UnitTests;
public class ValidateEmailTests
{
    [Theory]
    [InlineData(null, false)]
    [InlineData("", false)]
    [InlineData("valid@example.com", true)]
    [InlineData("user.name@domain.com", true)]
    [InlineData("invalid", false)]
    [InlineData("missing@tld", false)]
    [InlineData("@domain.com", false)]
    public void BeValidEmailTestCases(string? email, bool expectedResult)
    {
        var result = ValidateEmail.BeValidEmail(email);
        
        Assert.Equal(expectedResult, result);
    }
}