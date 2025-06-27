using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Security.Claims;
using System.Text.Json;
using User.Application.Services;
using User.Domain.Models.Requests;
using User.Domain.Models.Response;
using UserService.Controllers;
using Xunit;

namespace UserService.UnitTests.Controllers
{
    public class UserControllerTests
    {
        private readonly Mock<IUserService> _mockUserService;
        private readonly UserController _controller;

        public UserControllerTests()
        {
            _mockUserService = new Mock<IUserService>();
            _controller = new UserController(_mockUserService.Object);
        }

        private void SetUserContext(int userId)
        {
            var user = new ClaimsPrincipal(new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.NameIdentifier, userId.ToString())
            }, "mock"));

            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = user }
            };
        }

        [Fact]
        public void GetUserData_ReturnsOk_WhenUserExists()
        {
            // Arrange
            SetUserContext(1);
            var userDto = new UserResponseDTO { Id = 1, Email = "test@example.com" };
            _mockUserService.Setup(s => s.GetUser(1)).Returns(userDto);

            // Act
            var result = _controller.GetUserData();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedUser = Assert.IsType<UserResponseDTO>(okResult.Value);
            Assert.Equal(1, returnedUser.Id);
        }

        [Fact]
        public void GetUserData_ReturnsNotFound_OnException()
        {
            // Arrange
            SetUserContext(1);
            _mockUserService.Setup(s => s.GetUser(It.IsAny<int>())).Throws<Exception>();

            // Act
            var result = _controller.GetUserData();

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public void Register_ReturnsOk_WhenSuccess()
        {
            // Arrange
            var mockUserService = new Mock<IUserService>();
            var controller = new UserController(mockUserService.Object);

            var dto = new RegisterUserDTO
            {
                Email = "test@example.com",
                PasswordHash = "password123",
            };

            // Act
            var result = controller.Register(dto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);

            // Serializacja i deserializacja do słownika
            var json = JsonSerializer.Serialize(okResult.Value);
            var response = JsonSerializer.Deserialize<Dictionary<string, string>>(json);

            Assert.NotNull(response);
            Assert.True(response.ContainsKey("message"));
            Assert.Equal("User registered successfully", response["message"]);
        }


        [Fact]
        public void RegisterReturnsBadRequestOnException()
        {
            // Arrange
            var mockUserService = new Mock<IUserService>();
            var controller = new UserController(mockUserService.Object);

            var dto = new RegisterUserDTO
            {
                Email = "test@example.com",
                PasswordHash = "password123",
            };

            mockUserService
                .Setup(s => s.RegisterUser(It.IsAny<RegisterUserDTO>()))
                .Throws(new Exception("Test error"));

            // Act
            var result = controller.Register(dto);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);

            // Serializujemy i deserializujemy anonimowy obiekt
            var json = JsonSerializer.Serialize(badRequestResult.Value);
            var response = JsonSerializer.Deserialize<Dictionary<string, string>>(json);

            Assert.NotNull(response);
            Assert.True(response.ContainsKey("error"));
            Assert.Equal("Test error", response["error"]);
        }

        [Fact]
        public void ResetPassword_ReturnsOk()
        {
            // Arrange
            var mockUserService = new Mock<IUserService>();
            var controller = new UserController(mockUserService.Object);

            var dto = new ResetPasswordDTO
            {
                Email = "test@example.com",
                NewPassword = "newpassword123"
            };

            // Act
            var result = controller.ResetPassword(dto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);

            var json = JsonSerializer.Serialize(okResult.Value);
            var response = JsonSerializer.Deserialize<Dictionary<string, string>>(json);

            Assert.NotNull(response);
            Assert.True(response.ContainsKey("message"));
            Assert.Equal("Password has been reset successfully.", response["message"]);
        }


        [Fact]
        public void EditProfile_ReturnsOk()
        {
            // Arrange
            SetUserContext(5);
            var dto = new EditUserDTO { Username = "Test" };

            // Act
            var result = _controller.EditProfile(dto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var json = JsonSerializer.Serialize(okResult.Value);
            Assert.Contains("Profile updated successfully", json);
        }

        [Fact]
        public async Task DeleteUser_ReturnsNoContent_WhenSuccess()
        {
            // Arrange
            _mockUserService.Setup(s => s.DeleteUserAsync(1)).ReturnsAsync(true);

            // Act
            var result = await _controller.DeleteUser(1);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task DeleteUser_ReturnsNotFound_WhenNotFound()
        {
            // Arrange
            _mockUserService.Setup(s => s.DeleteUserAsync(1)).ReturnsAsync(false);

            // Act
            var result = await _controller.DeleteUser(1);

            // Assert
            var notFound = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("User with id 1 not found.", notFound.Value);
        }
    }
}
