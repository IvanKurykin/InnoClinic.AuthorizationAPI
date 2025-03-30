using DAL.Constants;
using DAL.Context;
using DAL.Entities;
using DAL.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace DAL.Repositories;

public class AuthRepository(UserManager<User> userManager, ApplicationDbContext dbContext, SignInManager<User> signInManager) : IAuthRepository
{
    public async Task<IdentityResult> RegisterAsync(User user, string password, CancellationToken cancellationToken = default)
    {
        await userManager.CreateAsync(user, password);
        await dbContext.SaveChangesAsync();
        await userManager.AddToRoleAsync(user, Roles.Patient);

        return IdentityResult.Success;
    }

    public async Task<SignInResult> LogInAsync(string email, string password, bool rememberMe, CancellationToken cancellationToken = default)
    {
        var user = await userManager.FindByEmailAsync(email);
        return await signInManager.PasswordSignInAsync(user?.UserName, password, rememberMe, lockoutOnFailure: false);
    }
}