using AutoMapper;
using BLL.DTO;
using BLL.Exceptions;
using BLL.Helpers;
using BLL.Interfaces;
using DAL.Entities;
using DAL.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace BLL.Services;

public class AuthService(IAuthRepository authRepository, IMapper mapper, IJwtTokenHelper jwtTokenHelper, IHttpContextAccessor httpContextAccessor) : IAuthService
{
    public async Task<IdentityResult> RegisterAsync(RegisterDto dto, CancellationToken cancellationToken = default)
    {
        ValidationHelper.ThrowIfNull((dto.Password, nameof(dto.Password)), (dto.UserName, nameof(dto.UserName)), (dto.Role, nameof(dto.Role)));

        var user = mapper.Map<User>(dto);

        return await authRepository.RegisterAsync(user, dto.Password ?? string.Empty, dto.Role ?? string.Empty, cancellationToken);
    }

    public async Task<string> LogInAsync(LogInDto dto, CancellationToken cancellationToken = default)
    {
        ValidationHelper.ThrowIfNull((dto.Email, nameof(dto.Email)), (dto.Password, nameof(dto.Password)), (dto.Role, nameof(dto.Role)));

        var user = await authRepository.GetUserByEmailAsync(dto.Email ?? string.Empty, cancellationToken);

        if (user is null)  throw new UserNotFoundException();
        if (user.UserName is null) throw new InvalidOperationException();

        var roles = await authRepository.GetUserRolesAsync(user, cancellationToken);

        if (dto.Role != roles.Single()) throw new ForbiddenAccessException();

        var signInResult = await authRepository.LogInAsync(user.UserName, dto.Password ?? string.Empty, dto.RememberMe, cancellationToken);

        if (!signInResult.Succeeded) throw new UserIsNotLoggedInException();
        
        return jwtTokenHelper.GenerateJwtToken(dto.Email ?? string.Empty, dto.Role);
    } 

    public async Task LogOutAsync(CancellationToken cancellationToken)
    {
        await authRepository.LogOutAsync(cancellationToken);

        var response = httpContextAccessor.HttpContext?.Response;

        if (response is not null) response.Cookies.Delete("jwt");
    }
}
