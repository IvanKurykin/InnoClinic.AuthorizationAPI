using System.Security.Authentication;
using API;
using AutoMapper;
using BLL.DTO;
using BLL.Exceptions;
using BLL.Interfaces;
using DAL.Entities;
using DAL.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace BLL.Services;

public class AuthService(IAuthRepository authRepository, IMapper mapper) : IAuthService
{
    public async Task<IdentityResult> RegisterAsync(RegisterDto dto, CancellationToken cancellationToken = default)
    {
        if (dto.Password is null) throw new ArgumentNullException(nameof(dto.Password));
        if (dto.UserName is null) throw new ArgumentNullException(nameof(dto.UserName));
        
        var user = mapper.Map<User>(dto);

        return await authRepository.RegisterAsync(user, dto.Password, cancellationToken);
    }

    public async Task<SignInResult> LogInAsync(LogInDto dto, CancellationToken cancellationToken = default)
    {
        if (dto.Email is null) throw new ArgumentNullException(nameof(dto.Email));
        if (dto.Password is null) throw new ArgumentNullException(nameof(dto.Password));

        var user = await authRepository.GetUserByEmailAsync(dto.Email, cancellationToken);

        if (user is null)  throw new UserNotFoundException(); 
        if (user.UserName is null) throw new InvalidOperationException();

        return await authRepository.LogInAsync(user.UserName, dto.Password, dto.RememberMe, cancellationToken);
    }
}
