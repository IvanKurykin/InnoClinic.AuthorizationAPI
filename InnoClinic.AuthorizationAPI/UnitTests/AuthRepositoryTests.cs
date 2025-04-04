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

    public static IEnumerable<object[]> LoginTestCases()
    {
        yield return new object[] { "user1", "pass1", false, SignInResult.Success, true };
        yield return new object[] { "user2", "pass2", true, SignInResult.Failed, false };
        yield return new object[] { "user3", "pass3", false, SignInResult.LockedOut, false };
    }

    public static IEnumerable<object?[]> UserEmailTestCases()
    {
        yield return new object?[] { "nonexistent@example.com", null };
        yield return new object?[] { "existing@example.com",
                new User { Email = "existing@example.com", UserName = TestConstans.TestUserName } };
    }

    [Fact]
    public async Task RegisterAsyncShouldRegisterUserWithRole()
    {
        var user = new User { Email = TestConstans.TestUserEmail, UserName = TestConstans.TestUserName };
        var password = TestConstans.TestUserPassword;
        var role = Roles.Patient;

        _userManager.Setup(x => x.CreateAsync(It.IsAny<User>(), It.IsAny<string>())).ReturnsAsync(IdentityResult.Success);
        _userManager.Setup(x => x.AddToRoleAsync(It.IsAny<User>(), It.IsAny<string>())).ReturnsAsync(IdentityResult.Success);

        var result = await _authRepository.RegisterAsync(user, password, role, CancellationToken.None);

        result.Should().NotBeNull();
        result.Succeeded.Should().BeTrue();
    }

    [Theory]
    [MemberData(nameof(LoginTestCases))]
    public async Task LogInAsyncShouldReturnExpectedResult(string userName, string password, bool rememberMe, SignInResult expectedResult, bool expectedSuccess)
    {
        _signInManager.Setup(x => x.PasswordSignInAsync(userName, password, rememberMe, false)).ReturnsAsync(expectedResult);

        var result = await _authRepository.LogInAsync(userName, password, rememberMe, CancellationToken.None);

        result.Should().NotBeNull();
        result.Succeeded.Should().Be(expectedSuccess);
    }

    [Theory]
    [MemberData(nameof(UserEmailTestCases))]
    public async Task GetUserByEmailAsyncShouldReturnExpectedUser(string email, User expectedUser)
    {
        _userManager.Setup(x => x.FindByEmailAsync(email)).ReturnsAsync(expectedUser);

        var result = await _authRepository.GetUserByEmailAsync(email, CancellationToken.None);

        result.Should().Be(expectedUser);
    }

    [Fact]
    public async Task GetUserRolesAsyncShouldReturnUserRoles()
    {
        var user = new User { Email = TestConstans.TestUserEmail, UserName = TestConstans.TestUserName };
        var expectedRoles = new List<string> { Roles.Patient };

        _userManager.Setup(x => x.GetRolesAsync(user)).ReturnsAsync(expectedRoles);

        var result = await _authRepository.GetUserRolesAsync(user, CancellationToken.None);

        result.Should().BeEquivalentTo(expectedRoles);
    }

    [Fact]
    public async Task LogOutAsyncShouldCallSignOut()
    {
        await _authRepository.LogOutAsync(CancellationToken.None);
        _signInManager.Verify(x => x.SignOutAsync(), Times.Once);
    }
}