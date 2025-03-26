using System.Runtime.CompilerServices;
using DAL.Context;
using DAL.Entities;
using DAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories;

public class UserRepository : IUserRepository
{
    private readonly ApplicationDbContext _applicationDbContext;
    public UserRepository(ApplicationDbContext applicationDbContext) => _applicationDbContext = applicationDbContext;

    public async Task AddUserAsync(User user, CancellationToken cancellationToken = default)
    {
        await _applicationDbContext.AddAsync(user, cancellationToken);
        await _applicationDbContext.SaveChangesAsync();
    }
    public async Task<User?> GetUserByEmailAsync(string email, CancellationToken cancellationToken = default) =>
        await _applicationDbContext.Users.FirstOrDefaultAsync(u => u.Email == email, cancellationToken);

}
