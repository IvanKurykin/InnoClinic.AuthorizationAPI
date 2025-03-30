using BLL.DTO;
using BLL.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController(IAuthService authService) : ControllerBase
{
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [HttpPost("register")]
    public async Task<IdentityResult> RegisterAsync([FromBody] RegisterDto dto, CancellationToken cancellationToken) =>
        await authService.RegisterAsync(dto, cancellationToken);

    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [HttpPost("login")]
    public async Task<SignInResult> LogInAsyn([FromBody] LogInDto dto, CancellationToken cancellationToken) =>
        await authService.LogInAsync(dto, cancellationToken);
}
