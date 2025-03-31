using DAL.Context;
using DAL.Entities;
using DAL.Repositories;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
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
        _userManager = new Mock<UserManager<User>>(storeMock.Object, null, null, null, null, null, null, null, null); 

       
        _signInManager = new Mock<SignInManager<User>>(_userManager.Object, Mock.Of<IHttpContextAccessor>(), Mock.Of<IUserClaimsPrincipalFactory<User>>(), null,   null,   null); 

        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _dbContext = new ApplicationDbContext(options);

        _authRepository = new AuthRepository(_userManager.Object, _dbContext, _signInManager.Object);
    }

    [Fact]
    public async Task RegisterAsync()
    {
        var user = new User
        {
            Email = "test@example.com",
            UserName = "test@example.com"
        };
        var password = "Test@123";
        var cancellationToken = CancellationToken.None;

        _userManager.Setup(x => x.CreateAsync(It.IsAny<User>(), It.IsAny<string>())).ReturnsAsync(IdentityResult.Success);
        _userManager.Setup(x => x.AddToRoleAsync(It.IsAny<User>(), It.IsAny<string>())).ReturnsAsync(IdentityResult.Success);

        var result = await _authRepository.RegisterAsync(user, password, cancellationToken);

        result.Should().NotBeNull();
        result.Succeeded.Should().BeTrue();
    }
    [Fact]
    public async Task LogInAsync()
    {
        var userName = "test@example.com";
        var password = "Test@123";
        var rememberMe = false;
        var cancellationToken = new CancellationToken();

        _signInManager.Setup(x => x.PasswordSignInAsync(userName, password, rememberMe, false)).ReturnsAsync(SignInResult.Success);

        var result = await _authRepository.LogInAsync(userName, password, rememberMe, cancellationToken);

        result.Should().NotBeNull();
        result.Succeeded.Should().BeTrue();
    }

    [Fact]
    public async Task GetUserByEmailAsync()
    {
        var email = "existing@example.com";
        var expectedUser = new User { Email = email, UserName = "existing" };
        var cancellationToken = new CancellationToken();

        _userManager.Setup(x => x.FindByEmailAsync(email)).ReturnsAsync(expectedUser);

        var result = await _authRepository.GetUserByEmailAsync(email, cancellationToken);

        result.Should().NotBeNull();
        result.Email.Should().Be(email);
    }
}