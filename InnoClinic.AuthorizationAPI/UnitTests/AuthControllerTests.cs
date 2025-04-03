using API;
using API.Controllers;
using BLL.DTO;
using BLL.Interfaces;
using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Moq;

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
    public async Task LogOutAsAWorkerAsyncShouldReturnOkWhenAuthorized()
    {
        var cancellationToken = new CancellationToken();

        var result = await _authController.LogOutAsAWorkerAsync(cancellationToken);

        result.Should().BeOfType<OkObjectResult>();
        var okResult = result as OkObjectResult;
        okResult!.Value.Should().Be(Messages.UserLoggedOutSuccessfully);
    }
}