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
        var user = new User { Email = TestConstans.TestUserEmail, UserName = TestConstans.TestUserName };
        var password = TestConstans.TestUserPassword;
        var role = Roles.Patient;

        _userManager.Setup(x => x.CreateAsync(It.IsAny<User>(), It.IsAny<string>())).ReturnsAsync(IdentityResult.Success);
        _userManager.Setup(x => x.AddToRoleAsync(It.IsAny<User>(), It.IsAny<string>())).ReturnsAsync(IdentityResult.Success);

        var result = await _authRepository.RegisterAsync(user, password, role, CancellationToken.None);

        result.Should().NotBeNull();
        result.Succeeded.Should().BeTrue();
    }

    [Fact]
    public async Task LogInAsyncShouldReturnExpectedResult()
    {
        var expectedResult = SignInResult.Success;

        _signInManager.Setup(x => x.PasswordSignInAsync( It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>())).ReturnsAsync(SignInResult.Success);

        var result = await _authRepository.LogInAsync(TestConstans.TestUserName, TestConstans.TestUserPassword, false, CancellationToken.None);

        result.Should().NotBeNull();
        result.Should().Be(expectedResult);
    }

    [Fact]
    public async Task GetUserByEmailAsyncShouldReturnExpectedUser()
    {
        var expectedUser = new User { Email = TestConstans.TestUserEmail }; 

        _userManager.Setup(x => x.FindByEmailAsync(TestConstans.TestUserEmail)).ReturnsAsync(expectedUser);

        var result = await _authRepository.GetUserByEmailAsync(TestConstans.TestUserEmail, CancellationToken.None);

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