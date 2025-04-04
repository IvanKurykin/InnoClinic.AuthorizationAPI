using BLL.DTO;

public class NullRegisterFields : TheoryData<string>
{
    public NullRegisterFields()
    {
        Add(nameof(RegisterDto.UserName));
        Add(nameof(RegisterDto.Password));
        Add(nameof(RegisterDto.Role));
    }
}
