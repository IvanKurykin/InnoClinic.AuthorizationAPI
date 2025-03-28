using DAL.Constants;

namespace BLL.DTO;

public sealed record class RegisterDto
{
    public string? Email { get; set; }
    public string? Password { get; set; }
    public string? ReEnteredPassword { get; set; }
    public string Role { get; set; } = Roles.Patient;
}
