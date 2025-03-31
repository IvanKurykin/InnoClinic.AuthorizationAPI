using System.Threading;
using AutoMapper;
using BLL.DTO;
using BLL.Services;
using DAL.Entities;
using DAL.Interfaces;
using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Moq;

namespace UnitTests;

public class AuthServiceTests
{
    private readonly Mock<IAuthRepository> _authRepositoryMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly AuthService _authService;

    public AuthServiceTests()
    {
        _authRepositoryMock = new Mock<IAuthRepository>();
        _mapperMock = new Mock<IMapper>();
        _authService = new AuthService(_authRepositoryMock.Object, _mapperMock.Object);
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

        var user = new User
        {
            UserName = dto.UserName,
            Email = dto.Email
        };

        var cancellationToken = new CancellationToken();

        _mapperMock.Setup(x => x.Map<User>(dto)).Returns(user);
        _authRepositoryMock.Setup(x => x.RegisterAsync(user, dto.Password, It.IsAny<CancellationToken>()))
            .ReturnsAsync(IdentityResult.Success);

        var result = await _authService.RegisterAsync(dto, cancellationToken);

        result.Should().NotBeNull();
        result.Succeeded.Should().BeTrue();
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

        var user = new User
        {
            UserName = TestConstans.TestUserName,
            Email = dto.Email
        };

        var cancellationToken = new CancellationToken();

        _authRepositoryMock.Setup(x => x.GetUserByEmailAsync(dto.Email, It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);
        _authRepositoryMock.Setup(x => x.LogInAsync(user.UserName!, dto.Password, dto.RememberMe, It.IsAny<CancellationToken>()))
            .ReturnsAsync(SignInResult.Success);

        var result = await _authService.LogInAsync(dto, cancellationToken);

        result.Should().NotBeNull();
        result.Succeeded.Should().BeTrue();
    }
}