using AutoMapper;
using BLL.Interfaces;
using BLL.DTO;
using DAL.Entities;
using DAL.Interfaces;
using Microsoft.AspNetCore.Identity;
using BLL.Exceptions;

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
        return await authRepository.LogInAsync(dto.Email, dto.Password, dto.RememberMe);
    }
}
