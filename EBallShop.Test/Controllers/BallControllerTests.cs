using AutoMapper;
using EBallShop.Controllers;
using EShop.Application.Services;
using EShop.Domain.ModelsDto;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

public class BallControllerTests
{
    private readonly Mock<IBallService> _ballServiceMock;
    private readonly Mock<ILogger<BallController>> _loggerMock;
    private readonly BallController _controller;

    public BallControllerTests()
    {
        _ballServiceMock = new Mock<IBallService>();
        _loggerMock = new Mock<ILogger<BallController>>();
        _controller = new BallController(_ballServiceMock.Object, _loggerMock.Object);
    }


    [Fact]
    public void Get_ReturnsNotFound_WhenBallDoesNotExist()
    {
        _ballServiceMock.Setup(s => s.GetById(999)).Returns((BallDto)null);

        // Act
        var result = _controller.Get(999);

        // Assert
        result.Result.Should().BeOfType<NotFoundResult>();
    }

    [Fact]
    public void CreateBall_ReturnsCreated_WhenValid()
    {
        // Arrange
        var createDto = new CreateBallDto { Name = "NewBall", Size = "Small", Description = "Test" };
        var createdId = 5;

        _ballServiceMock.Setup(s => s.Create(createDto)).Returns(createdId);
        _ballServiceMock.Setup(s => s.GetById(createdId)).Returns(new BallDto
        {
            Id = createdId,
            Name = "NewBall",
            Size = "Small",
            Description = "Test"
        });

        // Act
        var result = _controller.CreateBall(createDto);

        // Assert
        var createdResult = result as CreatedResult;
        createdResult.Should().NotBeNull();
        createdResult!.Location.Should().Be($"/api/ball/{createdId}");
    }

    [Fact]
    public void Update_ReturnsOk_WhenUpdated()
    {
        // Arrange
        var id = 1;
        var dto = new UpdateBallDto { Name = "Updated", Size = "XL", Description = "UpdatedDesc" };

        _ballServiceMock.Setup(s => s.GetById(id)).Returns(new BallDto { Id = id, Name = "Old", Size = "M", Description = "OldDesc" });
        _ballServiceMock.Setup(s => s.Update(id, dto)).Returns(true);
        _ballServiceMock.Setup(s => s.GetById(id)).Returns(new BallDto { Id = id, Name = "Updated", Size = "XL", Description = "UpdatedDesc" });

        // Act
        var result = _controller.Update(dto, id);

        // Assert
        result.Should().BeOfType<OkResult>();
    }

    [Fact]
    public void Update_ReturnsNotFound_WhenNotFound()
    {
        // Arrange
        var id = 123;
        var dto = new UpdateBallDto { Name = "X", Size = "L", Description = "Y" };

        _ballServiceMock.Setup(s => s.GetById(id)).Returns(new BallDto());
        _ballServiceMock.Setup(s => s.Update(id, dto)).Returns(false);

        // Act
        var result = _controller.Update(dto, id);

        // Assert
        result.Should().BeOfType<NotFoundResult>();
    }

    [Fact]
    public void Delete_ReturnsNoContent_WhenDeleted()
    {
        var id = 1;

        _ballServiceMock.Setup(s => s.GetById(id)).Returns(new BallDto { Id = id, Name = "X", Size = "M", Description = "Y" });
        _ballServiceMock.Setup(s => s.Delete(id)).Returns(true);

        // Act
        var result = _controller.Delete(id);

        // Assert
        result.Should().BeOfType<NoContentResult>();
    }

    [Fact]
    public void Delete_ReturnsNotFound_WhenNotExists()
    {
        var id = 404;

        _ballServiceMock.Setup(s => s.GetById(id)).Returns((BallDto)null);
        _ballServiceMock.Setup(s => s.Delete(id)).Returns(false);

        var result = _controller.Delete(id);

        result.Should().BeOfType<NotFoundResult>();
    }
}
