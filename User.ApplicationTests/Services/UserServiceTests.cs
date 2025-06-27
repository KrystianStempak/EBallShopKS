using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Threading.Tasks;
using User.Application.Services;
using User.Domain.Models.Entities;
using User.Domain.Models.Requests;
using User.Domain.Models.Response;
using Xunit;

public class UserServiceTests
{
    private readonly IMapper _mapper;
    private readonly UserDbContext _dbContext;
    private readonly Mock<IPasswordHasher<User.Domain.Models.Entities.User>> _passwordHasherMock;
    private readonly IUserService _service;

    public UserServiceTests()
    {
        // Konfiguracja InMemory DbContext
        var options = new DbContextOptionsBuilder<UserDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        _dbContext = new UserDbContext(options);

        // Konfiguracja AutoMapper - prosty mapowanie między encją i DTO
        var config = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<User.Domain.Models.Entities.User, UserResponseDTO>();
            cfg.CreateMap<RegisterUserDTO, User.Domain.Models.Entities.User>();
        });
        _mapper = config.CreateMapper();

        _passwordHasherMock = new Mock<IPasswordHasher<User.Domain.Models.Entities.User>>();
        _passwordHasherMock
            .Setup(x => x.HashPassword(It.IsAny<User.Domain.Models.Entities.User>(), It.IsAny<string>()))
            .Returns("hashed_password");

        _service = new User.Application.Services.UserService(_mapper, _dbContext, _passwordHasherMock.Object);
    }

    [Fact]
    public void GetUser_ThrowsException_WhenUserDoesNotExist()
    {
        Assert.Throws<Exception>(() => _service.GetUser(999));
    }

    [Fact]
    public void ResetPassword_UpdatesPasswordHash_WhenUserExists()
    {
        // Arrange
        var user = new User.Domain.Models.Entities.User
        {
            Id = 1,
            Username = "testuser", // wymagane!
            Email = "test@example.com",
            PasswordHash = "old_hash",
            RoleId = 2
        };

        _dbContext.Users.Add(user);
        _dbContext.SaveChanges();

        var dto = new ResetPasswordDTO
        {
            Email = "test@example.com",
            NewPassword = "newpassword123"
        };

        _passwordHasherMock
            .Setup(h => h.HashPassword(user, dto.NewPassword))
            .Returns("new_hashed_password");

        // Act
        _service.ResetPassword(dto);

        // Assert
        var updatedUser = _dbContext.Users.FirstOrDefault(u => u.Email == dto.Email);
        Assert.NotNull(updatedUser);
        Assert.Equal("new_hashed_password", updatedUser.PasswordHash);
    }


    [Fact]
    public void ResetPassword_ThrowsException_WhenUserNotFound()
    {
        var dto = new ResetPasswordDTO { Email = "notfound@example.com", NewPassword = "pass" };
        Assert.Throws<Exception>(() => _service.ResetPassword(dto));
    }

    [Fact]
    public void EditUser_UpdatesUser_WhenValid()
    {
        // Arrange
        var user = new User.Domain.Models.Entities.User
        {
            Id = 1,
            Username = "oldusername",
            Email = "old@example.com",
            PasswordHash = "hash", // wymagane!
            RoleId = 2
        };

        _dbContext.Users.Add(user);
        _dbContext.SaveChanges();

        var dto = new EditUserDTO
        {
            Username = "newusername",
            Email = "new@example.com"
        };

        // Act
        _service.EditUser(user.Id, dto);

        // Assert
        var updatedUser = _dbContext.Users.Find(user.Id);
        Assert.NotNull(updatedUser);
        Assert.Equal("newusername", updatedUser.Username);
        Assert.Equal("new@example.com", updatedUser.Email);
    }


    [Fact]
    public void EditUser_ThrowsException_WhenUserNotFound()
    {
        var dto = new EditUserDTO { Username = "x", Email = "x@test.com" };
        Assert.Throws<Exception>(() => _service.EditUser(999, dto));
    }



    [Fact]
    public void EditUser_ThrowsException_WhenEmailTaken()
    {
        // Arrange
        var existingUser = new User.Domain.Models.Entities.User
        {
            Id = 1,
            Username = "user1",
            Email = "existing@example.com",
            PasswordHash = "hashed_pass1",
            RoleId = 2
        };

        var userToEdit = new User.Domain.Models.Entities.User
        {
            Id = 2,
            Username = "user2",
            Email = "user2@example.com",
            PasswordHash = "hashed_pass2",
            RoleId = 2
        };

        _dbContext.Users.Add(existingUser);
        _dbContext.Users.Add(userToEdit);
        _dbContext.SaveChanges();

        var dto = new EditUserDTO
        {
            Username = "newusername",
            Email = "existing@example.com" // kolizja emailu z existingUser
        };

        // Act & Assert
        var ex = Assert.Throws<Exception>(() => _service.EditUser(userToEdit.Id, dto));
        Assert.Equal("Email is already taken", ex.Message);
    }


    [Fact]
    public void RegisterUser_AddsUser_WhenUsernameNotTaken()
    {
        // Arrange
        var dto = new RegisterUserDTO { Username = "newuser", PasswordHash = "password", Email = "newuser@test.com" };

        // Act
        _service.RegisterUser(dto);

        // Assert
        var addedUser = _dbContext.Users.FirstOrDefaultAsync(u => u.Username == "newuser").Result;
        Assert.NotNull(addedUser);
        Assert.Equal("hashed_password", addedUser.PasswordHash);
    }

    [Fact]
    public void RegisterUser_ThrowsException_WhenUsernameTaken()
    {
        // Arrange
        var existingUser = new User.Domain.Models.Entities.User
        {
            Id = 1,
            Username = "existinguser",
            Email = "existing@example.com",
            PasswordHash = "hashed_pw", // wymagane!
            RoleId = 2
        };

        _dbContext.Users.Add(existingUser);
        _dbContext.SaveChanges();

        var registerDto = new RegisterUserDTO
        {
            Username = "existinguser", // ten sam username
            Email = "new@example.com",
            PasswordHash = "password123"
        };

        // Act & Assert
        var ex = Assert.Throws<Exception>(() => _service.RegisterUser(registerDto));
        Assert.Equal("Username already taken", ex.Message);
    }


    [Fact]
    public void EditUser_ThrowsException_WhenUsernameTaken()
    {
        // Arrange
        var user1 = new User.Domain.Models.Entities.User
        {
            Id = 1,
            Username = "user1",
            Email = "user1@example.com",
            PasswordHash = "hash1", // wymagane!
            RoleId = 2
        };

        var user2 = new User.Domain.Models.Entities.User
        {
            Id = 2,
            Username = "user2",
            Email = "user2@example.com",
            PasswordHash = "hash2", // wymagane!
            RoleId = 2
        };

        _dbContext.Users.AddRange(user1, user2);
        _dbContext.SaveChanges();

        var dto = new EditUserDTO
        {
            Username = "user2", // próba ustawienia istniejącej nazwy
            Email = "newemail@example.com"
        };

        // Act & Assert
        var ex = Assert.Throws<Exception>(() => _service.EditUser(1, dto));
        Assert.Equal("Username is already taken", ex.Message);
    }


    [Fact]
    public async Task DeleteUserAsync_RemovesUser_WhenUserExists()
    {
        // Arrange
        var user = new User.Domain.Models.Entities.User
        {
            Id = 1,
            Username = "testuser",
            Email = "test@example.com",            // wymagane pole
            PasswordHash = "hashed_password",      // wymagane pole
            RoleId = 2
        };

        _dbContext.Users.Add(user);
        _dbContext.SaveChanges();

        // Act
        var result = await _service.DeleteUserAsync(user.Id);

        // Assert
        Assert.True(result);
        Assert.Null(_dbContext.Users.Find(user.Id));
    }


    [Fact]
    public async Task DeleteUserAsync_ReturnsFalse_WhenUserNotFound()
    {
        var result = await _service.DeleteUserAsync(999);
        Assert.False(result);
    }
}
