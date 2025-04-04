using DAL.Constants;

namespace BLL.DTO;

public sealed record class LogInDto
{
    public string? Email { get; set; }
    public string? Password { get; set; }
    public bool RememberMe { get; set; } = false;
    public string? Role { get; set; } = Roles.Patient;
}
