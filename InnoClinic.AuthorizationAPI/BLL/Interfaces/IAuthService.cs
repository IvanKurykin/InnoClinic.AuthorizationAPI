using BLL.Models.DTOs;
using Microsoft.AspNetCore.Identity;

namespace BLL.Interfaces;

public interface IAuthService
{
    Task<IdentityResult> RegisterAsync(RegisterDTO dto, CancellationToken cancellationToken = default);
    Task<SignInResult> LoginAsync(LogInDTO dto, CancellationToken cancellationToken = default);
}
