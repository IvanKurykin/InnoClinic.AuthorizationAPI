using DAL.Constants;
using DAL.Context;
using DAL.Entities;
using DAL.Repositories;
using FluentAssertions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;

namespace UnitTests;

public class AuthRepositoryTests
{
    private readonly Mock<UserManager<User>> _userManager;
    private readonly Mock<SignInManager<User>> _signInManager;
    private readonly AuthRepository _authRepository;
    private readonly ApplicationDbContext _dbContext;

    public AuthRepositoryTests()
    {
        var storeMock = new Mock<IUserStore<User>>();
        _userManager = new Mock<UserManager<User>>(
            storeMock.Object,
            null!, null!, null!, null!, null!, null!, null!, null!);

        var contextAccessor = new Mock<IHttpContextAccessor>();
        var userPrincipalFactory = new Mock<IUserClaimsPrincipalFactory<User>>();
        var options = new Mock<IOptions<IdentityOptions>>();
        var logger = new Mock<ILogger<SignInManager<User>>>();
        var schemes = new Mock<IAuthenticationSchemeProvider>();
        var confirmation = new Mock<IUserConfirmation<User>>();

        _signInManager = new Mock<SignInManager<User>>(
            _userManager.Object,
            contextAccessor.Object,
            userPrincipalFactory.Object,
            options.Object,
            logger.Object,
            schemes.Object,
            confirmation.Object);

        var optionsDb = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _dbContext = new ApplicationDbContext(optionsDb);
        _authRepository = new AuthRepository(_userManager.Object, _dbContext, _signInManager.Object);
    }

    [Fact]
    public async Task RegisterAsyncShouldRegisterUserWithRole()
    {
        var user = new User
        {
            Email = TestConstans.TestUserEmail,
            UserName = TestConstans.TestUserName
        };
        var password = TestConstans.TestUserPassword;
        var role = Roles.Patient;
        var cancellationToken = CancellationToken.None;

        _userManager.Setup(x => x.CreateAsync(It.IsAny<User>(), It.IsAny<string>())).ReturnsAsync(IdentityResult.Success);
        _userManager.Setup(x => x.AddToRoleAsync(It.IsAny<User>(), It.IsAny<string>())).ReturnsAsync(IdentityResult.Success);

        var result = await _authRepository.RegisterAsync(user, password, role, cancellationToken);

        result.Should().NotBeNull();
        result.Succeeded.Should().BeTrue();
    }

    [Fact]
    public async Task LogInAsyncShouldReturnSuccessForValidCredentials()
    {
        var userName = TestConstans.TestUserName;
        var password = TestConstans.TestUserPassword;
        var rememberMe = false;
        var cancellationToken = new CancellationToken();

        _signInManager.Setup(x => x.PasswordSignInAsync(userName, password, rememberMe, false)).ReturnsAsync(SignInResult.Success);

        var result = await _authRepository.LogInAsync(userName, password, rememberMe, cancellationToken);

        result.Should().NotBeNull();
        result.Succeeded.Should().BeTrue();
    }

    [Fact]
    public async Task LogInAsyncShouldReturnFailedForInvalidCredentials()
    {
        const string userName = "invalid";
        const string password = "invalid";
        var rememberMe = false;
        var cancellationToken = new CancellationToken();

        _signInManager.Setup(x => x.PasswordSignInAsync(userName, password, rememberMe, false)).ReturnsAsync(SignInResult.Failed);

        var result = await _authRepository.LogInAsync(userName, password, rememberMe, cancellationToken);

        result.Succeeded.Should().BeFalse();
    }

    [Fact]
    public async Task GetUserByEmailAsyncShouldReturnNullWhenUserNotFound()
    {
        const string email = "nonexistent@example.com";
        var cancellationToken = new CancellationToken();

        _userManager.Setup(x => x.FindByEmailAsync(email)).ReturnsAsync((User?)null);

        var result = await _authRepository.GetUserByEmailAsync(email, cancellationToken);

        result.Should().BeNull();
    }

    [Fact]
    public async Task GetUserByEmailAsyncShouldReturnUserWhenUserExists()
    {
        const string email = "existing@example.com";
        var expectedUser = new User { Email = email, UserName = TestConstans.TestUserName };
        var cancellationToken = new CancellationToken();

        _userManager.Setup(x => x.FindByEmailAsync(email)).ReturnsAsync(expectedUser);

        var result = await _authRepository.GetUserByEmailAsync(email, cancellationToken);

        result.Should().NotBeNull();
        result.Email.Should().Be(email);
    }

    [Fact]
    public async Task GetUserRolesAsyncShouldReturnUserRoles()
    {
        var user = new User { Email = TestConstans.TestUserEmail, UserName = TestConstans.TestUserName };
        var expectedRoles = new List<string> { Roles.Patient };
        var cancellationToken = new CancellationToken();

        _userManager.Setup(x => x.GetRolesAsync(user)).ReturnsAsync(expectedRoles);

        var result = await _authRepository.GetUserRolesAsync(user, cancellationToken);

        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(expectedRoles);
    }

    [Fact]
    public async Task LogOutAsyncShouldCallSignOut()
    {
        var cancellationToken = new CancellationToken();

        _signInManager.Setup(x => x.SignOutAsync())
            .Returns(Task.CompletedTask);

        await _authRepository.LogOutAsync(cancellationToken);

        _signInManager.Verify(x => x.SignOutAsync(), Times.Once);
    }
}