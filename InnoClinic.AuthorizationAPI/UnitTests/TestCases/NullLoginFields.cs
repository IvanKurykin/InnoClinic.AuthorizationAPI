namespace UnitTests.TestData;

using BLL.DTO;

public class NullLoginFields : TheoryData<string>
{
    public NullLoginFields()
    {
        Add(nameof(LogInDto.Email));
        Add(nameof(LogInDto.Password));
        Add(nameof(LogInDto.Role));
    }
}
