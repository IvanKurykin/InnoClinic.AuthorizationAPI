using System.Diagnostics.CodeAnalysis;
using BLL.DTO;
using Microsoft.AspNetCore.Identity;

namespace BLL.Interfaces;

public interface IAuthService
{
    Task<IdentityResult> RegisterAsync(RegisterDto dto, CancellationToken cancellationToken = default);
    Task<string> LogInAsync(LogInDto dto, CancellationToken cancellationToken = default);
    Task LogOutAsync(CancellationToken cancellationToken = default);
}
