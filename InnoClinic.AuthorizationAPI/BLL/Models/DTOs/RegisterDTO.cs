using DAL.Constants;

namespace BLL.Models.DTOs;

public sealed record class RegisterDTO 
{
    public string Email { get; set; }
    public string Password { get; set; }
    public string ReEnteredPassword { get; set; }
    public string Role { get; set; } = Roles.Patient;
}
