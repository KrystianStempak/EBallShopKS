using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using User.Application.Services;
using User.Domain.Exceptions;
using User.Domain.Models.Entities;
using Xunit;

public class LoginServiceTests
{
    private readonly UserDbContext _dbContext;
    private readonly Mock<IJwtTokenService> _jwtTokenServiceMock;
    private readonly Mock<IPasswordHasher<User.Domain.Models.Entities.User>> _passwordHasherMock;
    private readonly LoginService _loginService;

    public LoginServiceTests()
    {
        var options = new DbContextOptionsBuilder<UserDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _dbContext = new UserDbContext(options);

        _jwtTokenServiceMock = new Mock<IJwtTokenService>();
        _passwordHasherMock = new Mock<IPasswordHasher<User.Domain.Models.Entities.User>>();

        _loginService = new LoginService(_jwtTokenServiceMock.Object, _dbContext, _passwordHasherMock.Object);
    }

    [Fact]
    public void Login_ReturnsToken_WhenCredentialsAreValid()
    {
        // Arrange
        var user = new User.Domain.Models.Entities.User
        {
            Id = 1,
            Username = "testuser",
            Email = "test@example.com",
            PasswordHash = "hashedpassword",
            Role = new Role { Id = 1, Name = "User" }
        };

        _dbContext.Users.Add(user);
        _dbContext.SaveChanges();

        _passwordHasherMock
            .Setup(p => p.VerifyHashedPassword(user, user.PasswordHash, "password"))
            .Returns(PasswordVerificationResult.Success);

        _jwtTokenServiceMock
            .Setup(j => j.GenerateToken(user.Id, It.IsAny<List<string>>()))
            .Returns("mocked_token");

        // Act
        var token = _loginService.Login("testuser", "password");

        // Assert
        Assert.Equal("mocked_token", token);
    }

    [Fact]
    public void Login_ThrowsInvalidCredentialsException_WhenUserDoesNotExist()
    {
        // Act & Assert
        Assert.Throws<InvalidCredentialsException>(() =>
        {
            _loginService.Login("nonexistent", "password");
        });
    }

    [Fact]
    public void Login_ThrowsInvalidCredentialsException_WhenPasswordIsIncorrect()
    {
        // Arrange
        var user = new User.Domain.Models.Entities.User
        {
            Id = 1,
            Username = "testuser",
            Email = "test@example.com",
            PasswordHash = "hashedpassword",
            Role = new Role { Id = 1, Name = "User" }
        };

        _dbContext.Users.Add(user);
        _dbContext.SaveChanges();

        _passwordHasherMock
            .Setup(p => p.VerifyHashedPassword(user, user.PasswordHash, "wrongpassword"))
            .Returns(PasswordVerificationResult.Failed);

        // Act & Assert
        Assert.Throws<InvalidCredentialsException>(() =>
        {
            _loginService.Login("testuser", "wrongpassword");
        });
    }
}
