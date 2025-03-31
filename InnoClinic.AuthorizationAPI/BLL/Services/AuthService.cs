using System.Globalization;
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
       if (dto.Password is null) throw new PasswordNullException();
       return await authRepository.RegisterAsync(mapper.Map<User>(dto), dto.Password);
    }
        
    public async Task<SignInResult> LogInAsync(LogInDto dto, CancellationToken cancellationToken = default)
    {
        if (dto.Email is null || dto.Password is null) throw new EmailAndPasswordNullException();

        var user = await authRepository.GetUserByEmailAsync(dto.Email, cancellationToken);

        if (user is null) throw new UserNotFoundException();

        if (user.UserName is null) throw new UserNameNullException();

        return await authRepository.LogInAsync(user.UserName, dto.Password, dto.RememberMe, cancellationToken);
    }
}
