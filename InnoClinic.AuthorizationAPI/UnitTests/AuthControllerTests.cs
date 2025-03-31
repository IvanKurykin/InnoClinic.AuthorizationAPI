using API.Controllers;
using BLL.DTO;
using BLL.Interfaces;
using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace UnitTests;

public class AuthControllerTests
{
    private readonly Mock<IAuthService> _authServiceMock;
    private readonly AuthController _authController;

    public AuthControllerTests()
    {
        _authServiceMock = new Mock<IAuthService>();
        _authController = new AuthController(_authServiceMock.Object);
    }

    [Fact]
    public async Task RegisterAsync()
    {
        var dto = new RegisterDto
        {
            UserName = TestConstans.TestUserName,
            Email = TestConstans.TestUserEmail,
            Password = TestConstans.TestUserPassword
        };
        var cancellationToken = new CancellationToken();

        var successResult = IdentityResult.Success;
        _authServiceMock.Setup(x => x.RegisterAsync(dto, It.IsAny<CancellationToken>()))
            .ReturnsAsync(successResult);

        var result = await _authController.RegisterAsync(dto, cancellationToken);

        result.Should().BeOfType<OkObjectResult>();
        var okResult = result as OkObjectResult;
        okResult!.Value.Should().Be(successResult);
    }

    [Fact]
    public async Task LogInAsync()
    {
        var dto = new LogInDto
        {
            Email = TestConstans.TestUserEmail,
            Password = TestConstans.TestUserPassword,
            RememberMe = false
        };
        var cancellationToken = new CancellationToken();

        var successResult = SignInResult.Success;
        _authServiceMock.Setup(x => x.LogInAsync(dto, It.IsAny<CancellationToken>()))
            .ReturnsAsync(successResult);

        var result = await _authController.LogInAsynс(dto, cancellationToken);

        result.Should().BeOfType<OkObjectResult>();
        var okResult = result as OkObjectResult;
        okResult!.Value.Should().Be(successResult);
    }
}