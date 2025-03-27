using DAL.Entities;
using Microsoft.AspNetCore.Identity;

namespace DAL.Interfaces;

public interface IAuthRepository
{
    Task<IdentityResult> RegisterAsync(User user, string password, string role);
    Task<SignInResult> LoginAsync(string email, string password, bool rememberMe);
}
