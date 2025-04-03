using BLL.DTO;
using BLL.Interfaces;
using DAL.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController(IAuthService authService) : ControllerBase
{
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [HttpPost("register")]
    public async Task<IActionResult> RegisterAsync([FromBody] RegisterDto dto, CancellationToken cancellationToken)
    {
        var result = await authService.RegisterAsync(dto, cancellationToken);
        if (!result.Succeeded) return BadRequest(result.Errors);
        return Ok(result);
    }

    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [HttpPost("login")]
    public async Task<IActionResult> LogInAsynс([FromBody] LogInDto dto, CancellationToken cancellationToken)
    {
        var token = await authService.LogInAsync(dto, cancellationToken);

        Response.Cookies.Append("jwt", token, new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Strict
        });

        return Ok(Messages.UserLoggedInSuccessfully);
    }

    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [HttpPost("login-worker")]
    public async Task<IActionResult> LogInAsAWorkerAsync([FromBody] LogInDto dto, CancellationToken cancellationToken)
    {
        var token = await authService.LogInAsAWorkerAsync(dto, cancellationToken);

        Response.Cookies.Append("jwt", token, new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Strict
        });

        return Ok(Messages.UserLoggedInSuccessfully);
    }

    [HttpPost("logout")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [Authorize(Roles = Roles.Patient)]
    public async Task<IActionResult> LogOutAsync(CancellationToken cancellationToken)
    {
        await authService.LogOutAsync(cancellationToken);
        return Ok(Messages.UserLoggedOutSuccessfully);
    }

    [HttpPost("logout-worker")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [Authorize(Roles = $"{Roles.Admin},{Roles.Doctor}")]
    public async Task<IActionResult> LogOutAsAWorkerAsync(CancellationToken cancellationToken)
    {
        await authService.LogOutAsync(cancellationToken);
        return Ok(Messages.UserLoggedOutSuccessfully);
    }
}