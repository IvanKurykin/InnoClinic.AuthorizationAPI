using AutoMapper;
using BLL.DTO;
using BLL.Exceptions;
using BLL.Helpers;
using BLL.Services;
using DAL.Constants;
using DAL.Entities;
using DAL.Interfaces;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Moq;

namespace UnitTests;

public class AuthServiceTests
{
    private readonly Mock<IAuthRepository> _authRepositoryMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<IJwtTokenHelper> _jwtTokenHelperMock;
    private readonly Mock<IHttpContextAccessor> _httpContextAccessorMock;
    private readonly AuthService _authService;

    public AuthServiceTests()
    {
        _authRepositoryMock = new Mock<IAuthRepository>();
        _mapperMock = new Mock<IMapper>();
        _jwtTokenHelperMock = new Mock<IJwtTokenHelper>();
        _httpContextAccessorMock = new Mock<IHttpContextAccessor>();
        _authService = new AuthService(_authRepositoryMock.Object, _mapperMock.Object, _jwtTokenHelperMock.Object, _httpContextAccessorMock.Object);
    }

    [Fact]
    public async Task RegisterAsyncShouldReturnSuccessWhenValidData()
    {
        var dto = new RegisterDto
        {
            UserName = TestConstans.TestUserName,
            Email = TestConstans.TestUserEmail,
            Password = TestConstans.TestUserPassword,
            Role = Roles.Admin
        };

        var user = new User
        {
            UserName = dto.UserName,
            Email = dto.Email
        };

        var cancellationToken = new CancellationToken();

        _mapperMock.Setup(x => x.Map<User>(dto)).Returns(user);
        _authRepositoryMock.Setup(x => x.RegisterAsync(user, dto.Password, dto.Role, cancellationToken)).ReturnsAsync(IdentityResult.Success);

        var result = await _authService.RegisterAsync(dto, cancellationToken);

        result.Should().NotBeNull();
        result.Succeeded.Should().BeTrue();
    }

    [Fact]
    public async Task LogInAsyncShouldReturnTokenWhenValidCredentials()
    {
        var dto = new LogInDto
        {
            Email = TestConstans.TestUserEmail,
            Password = TestConstans.TestUserPassword,
            RememberMe = false,
            Role = Roles.Admin
        };

        var user = new User
        {
            UserName = TestConstans.TestUserName,
            Email = dto.Email
        };

        var expectedToken = "generated.jwt.token";
        var cancellationToken = new CancellationToken();

        _authRepositoryMock.Setup(x => x.GetUserByEmailAsync(dto.Email, cancellationToken)).ReturnsAsync(user);
        _authRepositoryMock.Setup(x => x.GetUserRolesAsync(user, cancellationToken)).ReturnsAsync(new List<string> { Roles.Admin });
        _authRepositoryMock.Setup(x => x.LogInAsync(user.UserName!, dto.Password, dto.RememberMe, cancellationToken)).ReturnsAsync(SignInResult.Success);
        _jwtTokenHelperMock.Setup(x => x.GenerateJwtToken(dto.Email, dto.Role)).Returns(expectedToken);

        var result = await _authService.LogInAsync(dto, cancellationToken);

        result.Should().Be(expectedToken);
    }

    [Theory]
    [InlineData(nameof(RegisterDto.Password))]
    [InlineData(nameof(RegisterDto.UserName))]
    [InlineData(nameof(RegisterDto.Role))]
    public async Task RegisterAsyncShouldThrowArgumentNullExceptionWhenRequiredFieldIsNull(string propertyName)
    {
        var dto = new RegisterDto
        {
            UserName = propertyName == nameof(RegisterDto.UserName) ? null! : TestConstans.TestUserName,
            Email = TestConstans.TestUserEmail,
            Password = propertyName == nameof(RegisterDto.Password) ? null! : TestConstans.TestUserPassword,
            Role = propertyName == nameof(RegisterDto.Role) ? null! : Roles.Admin
        };

        var cancellationToken = new CancellationToken();

        await Assert.ThrowsAsync<ArgumentNullException>(() => _authService.RegisterAsync(dto, cancellationToken));
    }

    [Theory]
    [InlineData(nameof(LogInDto.Email))]
    [InlineData(nameof(LogInDto.Password))]
    [InlineData(nameof(LogInDto.Role))]
    public async Task LogInAsyncShouldThrowArgumentNullExceptionWhenRequiredFieldIsNull(string propertyName)
    {
        var dto = new LogInDto
        {
            Email = propertyName == nameof(LogInDto.Email) ? null! : TestConstans.TestUserEmail,
            Password = propertyName == nameof(LogInDto.Password) ? null! : TestConstans.TestUserPassword,
            Role = propertyName == nameof(LogInDto.Role) ? null! : Roles.Admin,
            RememberMe = false
        };

        var cancellationToken = new CancellationToken();

        await Assert.ThrowsAsync<ArgumentNullException>(() => _authService.LogInAsync(dto, cancellationToken));
    }

    [Fact]
    public async Task LogInAsyncShouldThrowUserNotFoundExceptionWhenUserNotFound()
    {
        var dto = new LogInDto
        {
            Email = TestConstans.TestUserFailedEmail,
            Password = TestConstans.TestUserPassword,
            Role = Roles.Admin,
            RememberMe = false
        };

        var cancellationToken = new CancellationToken();

        _authRepositoryMock.Setup(x => x.GetUserByEmailAsync(dto.Email, cancellationToken)).ReturnsAsync((User?)null);

        await Assert.ThrowsAsync<UserNotFoundException>(() => _authService.LogInAsync(dto, cancellationToken));
    }

    [Fact]
    public async Task LogInAsyncShouldThrowForbiddenAccessExceptionWhenRoleMismatch()
    {
        var dto = new LogInDto
        {
            Email = TestConstans.TestUserEmail,
            Password = TestConstans.TestUserPassword,
            Role = Roles.Admin,
            RememberMe = false
        };

        var user = new User { Email = dto.Email, UserName = TestConstans.TestUserName };
        var cancellationToken = new CancellationToken();

        _authRepositoryMock.Setup(x => x.GetUserByEmailAsync(dto.Email, cancellationToken)).ReturnsAsync(user);
        _authRepositoryMock.Setup(x => x.GetUserRolesAsync(user, cancellationToken)).ReturnsAsync(new List<string> { Roles.Patient });

        await Assert.ThrowsAsync<ForbiddenAccessException>(() => _authService.LogInAsync(dto, cancellationToken));
    }
}