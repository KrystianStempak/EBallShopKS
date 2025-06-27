using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using ShoppingCart.Controllers;
using ShoppingCart.Domain.Command;
using ShoppingCart.Domain.ModelsDto;
using ShoppingCart.Domain.Queries;
using Xunit;

public class CartControllerTests
{
    private readonly Mock<IMediator> _mediatorMock;
    private readonly CartController _controller;

    public CartControllerTests()
    {
        _mediatorMock = new Mock<IMediator>();
        _controller = new CartController(_mediatorMock.Object);
    }

    [Fact]
    public async Task AddProductToCart_ReturnsOk()
    {
        // Arrange
        var command = new AddProductToCartCommand { /* ustaw właściwości jeśli potrzebne */ };

        _mediatorMock.Setup(m => m.Send(command, It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _controller.AddProductToCart(command);

        // Assert
        var okResult = Assert.IsType<OkResult>(result);
        _mediatorMock.Verify(m => m.Send(command, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task RemoveProductFromCart_ReturnsOk()
    {
        // Arrange
        var command = new RemoveProductFromCartCommand { /* ustaw właściwości jeśli potrzebne */ };

        _mediatorMock.Setup(m => m.Send(command, It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _controller.RemoveProductFromCart(command);

        // Assert
        var okResult = Assert.IsType<OkResult>(result);
        _mediatorMock.Verify(m => m.Send(command, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task GetCart_ReturnsOk_WhenCartExists()
    {
        // Arrange
        int cartId = 123;
        var cartDto = new CartDto { /* wypełnij, jeśli masz model DTO */ };

        _mediatorMock.Setup(m => m.Send(It.Is<GetCartQuery>(q => q.CartId == cartId), It.IsAny<CancellationToken>()))
            .ReturnsAsync(cartDto);

        // Act
        var result = await _controller.GetCart(cartId);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(cartDto, okResult.Value);
        _mediatorMock.Verify(m => m.Send(It.IsAny<GetCartQuery>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task GetCart_ReturnsNotFound_WhenCartDoesNotExist()
    {
        // Arrange
        int cartId = 123;

        _mediatorMock.Setup(m => m.Send(It.Is<GetCartQuery>(q => q.CartId == cartId), It.IsAny<CancellationToken>()))
            .ReturnsAsync((CartDto)null);

        // Act
        var result = await _controller.GetCart(cartId);

        // Assert
        Assert.IsType<NotFoundResult>(result);
        _mediatorMock.Verify(m => m.Send(It.IsAny<GetCartQuery>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task GetAllCarts_ReturnsOk_WithListOfCarts()
    {
        // Arrange
        var carts = new List<CartDto>
        {
            new CartDto { /* ... */ },
            new CartDto { /* ... */ }
        };

        _mediatorMock.Setup(m => m.Send(It.IsAny<GetAllCartsQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(carts);

        // Act
        var result = await _controller.GetAllCarts();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(carts, okResult.Value);
        _mediatorMock.Verify(m => m.Send(It.IsAny<GetAllCartsQuery>(), It.IsAny<CancellationToken>()), Times.Once);
    }
}

// Uwaga: CartDto jest przykładową klasą DTO, którą musisz mieć w swoim projekcie
// jeśli nie masz jej jeszcze, zastąp ją odpowiednim modelem zwracanym przez zapytania.
