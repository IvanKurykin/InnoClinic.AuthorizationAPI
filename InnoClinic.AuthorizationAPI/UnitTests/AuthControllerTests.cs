using System.Security.Claims;
using API;
using API.Controllers;
using BLL.DTO;
using BLL.Interfaces;
using DAL.Constants;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace UnitTests;

public class AuthControllerTests
{
    private readonly Mock<IAuthService> _authServiceMock;
    private readonly AuthController _authController;
    private readonly DefaultHttpContext _httpContext;
    public AuthControllerTests()
    {
        _authServiceMock = new Mock<IAuthService>();
        _authController = new AuthController(_authServiceMock.Object);
        _httpContext = new DefaultHttpContext();
        _authController.ControllerContext = new ControllerContext { HttpContext = _httpContext };
    }

    [Fact]
    public async Task RegisterAsyncShouldReturnOkWhenRegistrationSucceeds()
    {
        var dto = new RegisterDto
        {
            UserName = TestConstans.TestUserName,
            Email = TestConstans.TestUserEmail,
            Password = TestConstans.TestUserPassword
        };
        var cancellationToken = new CancellationToken();

        var successResult = IdentityResult.Success;
        _authServiceMock.Setup(x => x.RegisterAsync(dto, cancellationToken)).ReturnsAsync(successResult);

        var result = await _authController.RegisterAsync(dto, cancellationToken);

        result.Should().BeOfType<OkObjectResult>();
        var okResult = result as OkObjectResult;
        okResult!.Value.Should().Be(successResult);
    }

    [Fact]
    public async Task LogInAsyncShouldSetJwtCookieAndReturnOk()
    {
        var dto = new LogInDto { Email = "test@example.com", Password = "Password123!" };
        var cancellationToken = new CancellationToken();
        var expectedToken = "mock-jwt-token";

        _authServiceMock.Setup(x => x.LogInAsync(dto, cancellationToken)).ReturnsAsync(expectedToken);

        var responseCookiesMock = new Mock<IResponseCookies>();
        var responseMock = new Mock<HttpResponse>();
        responseMock.Setup(r => r.Cookies).Returns(responseCookiesMock.Object);

        var contextMock = new Mock<HttpContext>();
        contextMock.Setup(c => c.Response).Returns(responseMock.Object);

        _authController.ControllerContext = new ControllerContext { HttpContext = contextMock.Object };

        var result = await _authController.LogInAsynс(dto, cancellationToken);

        result.Should().BeOfType<OkObjectResult>();

        responseCookiesMock.Verify(c => c.Append(
            "jwt",
            expectedToken,
            It.Is<CookieOptions>(options => options.HttpOnly && options.Secure && options.SameSite == SameSiteMode.Strict)
        ), Times.Once);
    }

    [Fact]
    public async Task LogOutAsAWorkerAsyncShouldReturnOkWhenAuthorized()
    {
        var cancellationToken = new CancellationToken();

        var result = await _authController.LogOutAsAWorkerAsync(cancellationToken);

        result.Should().BeOfType<OkObjectResult>();
        var okResult = result as OkObjectResult;
        okResult!.Value.Should().Be(Messages.UserLoggedOutSuccessfully);
    }

    [Fact]
    public async Task LogOutAsAWorkerAsyncShouldReturnOk_WhenAuthorized()
    {
        var cancellationToken = new CancellationToken();
        _httpContext.User = new ClaimsPrincipal(new ClaimsIdentity(new[] { new Claim(ClaimTypes.Role, Roles.Admin) }));

        var result = await _authController.LogOutAsAWorkerAsync(cancellationToken);
        
        result.Should().BeOfType<OkObjectResult>();
        (result as OkObjectResult)!.Value.Should().Be(Messages.UserLoggedOutSuccessfully);
    }
}