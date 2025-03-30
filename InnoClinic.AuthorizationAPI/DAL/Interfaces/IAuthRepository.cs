using DAL.Entities;
using Microsoft.AspNetCore.Identity;

namespace DAL.Interfaces;

public interface IAuthRepository
{
    Task<IdentityResult> RegisterAsync(User user, string password, CancellationToken cancellationToken = default);
    Task<SignInResult> LogInAsync(string email, string password, bool rememberMe, CancellationToken cancellationToken = default);
}
