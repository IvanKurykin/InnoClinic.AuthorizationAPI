using DAL.Entities;
using DAL.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace DAL.Repositories;

public class AuthRepository(UserManager<User> userManager, SignInManager<User> signInManager) : IAuthRepository
{
    public async Task<IdentityResult> RegisterAsync(User user, string password, string role)
    {
        var result = await userManager.CreateAsync(user, password);
        await userManager.AddToRoleAsync(user, role);
        return result;
    }

    public async Task<SignInResult> LoginAsync(string email, string password, bool rememberMe) =>
        await signInManager.PasswordSignInAsync(email, password, rememberMe, lockoutOnFailure: false);
}
