using DAL.Context;
using DAL.Entities;
using DAL.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories;

public class AuthRepository : IAuthRepository
{
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;
    public AuthRepository(UserManager<User> userManager, SignInManager<User> signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
    }

    public async Task<IdentityResult> RegisterAsync(User user, string password, string role)
    {
        var result = await _userManager.CreateAsync(user, password);
        await _userManager.AddToRoleAsync(user, role);
        return result;
    }
    public async Task<SignInResult> LoginAsync(string email, string password, bool rememberMe) =>
        await _signInManager.PasswordSignInAsync(email, password, rememberMe, lockoutOnFailure: false);
    
}
