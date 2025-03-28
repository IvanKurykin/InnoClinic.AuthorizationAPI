using BLL.DTO;
using Microsoft.AspNetCore.Identity;

namespace BLL.Interfaces;

public interface IAuthService
{
    Task<IdentityResult> RegisterAsync(RegisterDto dto, CancellationToken cancellationToken = default);
    Task<SignInResult> LoginAsync(LogInDto dto, CancellationToken cancellationToken = default);
}
