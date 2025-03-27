namespace BLL.Models.DTOs;

public sealed record class LogInDTO
{
    public string Email { get; set; }
    public string Password { get; set; }
    public bool RememberMe { get; set; } = false;
}
