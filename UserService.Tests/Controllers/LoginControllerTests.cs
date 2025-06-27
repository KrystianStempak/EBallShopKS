using Microsoft.AspNetCore.Mvc;
using Moq;
using User.Domain.Exceptions;
using User.Domain.Models.Requests;
using UserService.Controllers;
using User.Application.Services;
using Xunit;

public class LoginControllerTests
{
    private readonly Mock<ILoginService> _mockLoginService;
    private readonly LoginController _controller;

    public LoginControllerTests()
    {
        _mockLoginService = new Mock<ILoginService>();
        _controller = new LoginController(_mockLoginService.Object);
    }

    [Fact]
    public void Login_ReturnsOk_WithToken_WhenCredentialsAreValid()
    {
        // Arrange
        var loginRequest = new LoginRequest { Username = "user1", Password = "pass1" };
        var expectedToken = "fake-jwt-token";

        _mockLoginService
            .Setup(s => s.Login(loginRequest.Username, loginRequest.Password))
            .Returns(expectedToken);

        // Act
        var result = _controller.Login(loginRequest);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var json = System.Text.Json.JsonSerializer.Serialize(okResult.Value);
        var response = System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, string>>(json);
        Assert.NotNull(response);
        Assert.True(response.ContainsKey("token"));
        Assert.Equal(expectedToken, response["token"]);
    }

    [Fact]
    public void Login_ReturnsUnauthorized_WhenInvalidCredentials()
    {
        // Arrange
        var loginRequest = new LoginRequest { Username = "user1", Password = "wrongpass" };

        _mockLoginService
            .Setup(s => s.Login(loginRequest.Username, loginRequest.Password))
            .Throws(new InvalidCredentialsException());

        // Act
        var result = _controller.Login(loginRequest);

        // Assert
        Assert.IsType<UnauthorizedResult>(result);
    }

    [Fact]
    public void AdminPage_ReturnsOk_WithAdminData()
    {
        // Act
        var result = _controller.AdminPage();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal("Dane tylko dla administratora", okResult.Value);
    }
}
