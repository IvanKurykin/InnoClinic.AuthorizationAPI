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

    public async Task<SignInResult> LogInAsync(string userName, string password, bool rememberMe, CancellationToken cancellationToken = default) =>
        await signInManager.PasswordSignInAsync(userName, password, rememberMe, lockoutOnFailure: false); 

    public async Task<User?> GetUserByEmailAsync(string email, CancellationToken cancellationToken = default) =>
        await userManager.FindByEmailAsync(email);
}