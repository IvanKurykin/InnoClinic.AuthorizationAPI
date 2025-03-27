using AutoMapper;
using BLL.Interfaces;
using BLL.DTO;
using DAL.Entities;
using DAL.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace BLL.Services;

public class AuthService(IAuthRepository authRepository, IMapper mapper) : IAuthService
{
    public async Task<IdentityResult> RegisterAsync(RegisterDto dto, CancellationToken cancellationToken = default) =>
        await authRepository.RegisterAsync(mapper.Map<User>(dto), dto.Password, dto.Role);

    public async Task<SignInResult> LoginAsync(LogInDto dto, CancellationToken cancellationToken = default) => 
        await authRepository.LoginAsync(dto.Email, dto.Password, dto.RememberMe);
}
