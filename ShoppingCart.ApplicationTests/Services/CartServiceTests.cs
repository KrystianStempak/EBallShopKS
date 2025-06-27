using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Moq;
using ShoppingCart.Application.Services;
using ShoppingCart.Domain.Interfaces;
using ShoppingCart.Domain.Models;
using ShoppingCart.Domain.ModelsDto;
using ShoppingCart.Infrastructure.Repositories;
using Xunit;

public class CartServiceTests
{
    private readonly Mock<ICartRepository> _repositoryMock;
    private readonly Mock<IBallInfoProvider> _ballInfoProviderMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly CartService _cartService;

    public CartServiceTests()
    {
        _repositoryMock = new Mock<ICartRepository>();
        _ballInfoProviderMock = new Mock<IBallInfoProvider>();
        _mapperMock = new Mock<IMapper>();

        _cartService = new CartService(_repositoryMock.Object, _ballInfoProviderMock.Object, _mapperMock.Object);
    }

    [Fact]
    public void AddProductToCart_AddsNewBall_WhenBallNotExistsInCart()
    {
        // Arrange
        int cartId = 1;
        var ballToAdd = new ShoppingCart.Domain.Models.Ball { BallId = 100, Quantity = 2 };
        var cart = new Cart { Id = cartId, Balls = new List<ShoppingCart.Domain.Models.Ball>() };

        _ballInfoProviderMock.Setup(b => b.GetBallByIdAsync(ballToAdd.BallId))
            .Returns(Task.FromResult(ballToAdd));

        _repositoryMock.Setup(r => r.FindById(cartId))
            .Returns(cart);

        // Act
        _cartService.AddProductToCart(cartId, ballToAdd);

        // Assert
        Assert.Single(cart.Balls);
        Assert.Equal(ballToAdd.BallId, cart.Balls.First().BallId);
        Assert.Equal(ballToAdd.Quantity, cart.Balls.First().Quantity);
        _repositoryMock.Verify(r => r.Add(cart), Times.Once);
    }

    [Fact]
    public void AddProductToCart_IncreasesQuantity_WhenBallAlreadyExistsInCart()
    {
        // Arrange
        int cartId = 1;
        var existingBall = new ShoppingCart.Domain.Models.Ball { BallId = 100, Quantity = 1 };
        var ballToAdd = new ShoppingCart.Domain.Models.Ball { BallId = 100, Quantity = 3 };
        var cart = new Cart { Id = cartId, Balls = new List<ShoppingCart.Domain.Models.Ball> { existingBall } };

        _ballInfoProviderMock.Setup(b => b.GetBallByIdAsync(ballToAdd.BallId))
            .Returns(Task.FromResult(ballToAdd));

        _repositoryMock.Setup(r => r.FindById(cartId))
            .Returns(cart);

        // Act
        _cartService.AddProductToCart(cartId, ballToAdd);

        // Assert
        Assert.Single(cart.Balls);
        Assert.Equal(existingBall.BallId, cart.Balls.First().BallId);
        Assert.Equal(4, cart.Balls.First().Quantity); // 1 + 3
        _repositoryMock.Verify(r => r.Add(cart), Times.Once);
    }

    [Fact]
    public void RemoveProductFromCart_RemovesBall_WhenExists()
    {
        // Arrange
        int cartId = 1;
        int ballId = 100;
        var ball = new ShoppingCart.Domain.Models.Ball { BallId = ballId, Quantity = 2 };
        var cart = new Cart { Id = cartId, Balls = new List<ShoppingCart.Domain.Models.Ball> { ball } };

        _repositoryMock.Setup(r => r.FindById(cartId)).Returns(cart);

        // Act
        _cartService.RemoveProductFromCart(cartId, ballId);

        // Assert
        Assert.Empty(cart.Balls);
        _repositoryMock.Verify(r => r.Update(cart), Times.Once);
    }

    [Fact]
    public void RemoveProductFromCart_DoesNothing_WhenCartNotFound()
    {
        // Arrange
        int cartId = 1;
        int ballId = 100;

        _repositoryMock.Setup(r => r.FindById(cartId)).Returns((Cart)null);

        // Act
        _cartService.RemoveProductFromCart(cartId, ballId);

        // Assert
        _repositoryMock.Verify(r => r.Update(It.IsAny<Cart>()), Times.Never);
    }

    [Fact]
    public void RemoveProductFromCart_DoesNothing_WhenBallNotFound()
    {
        // Arrange
        int cartId = 1;
        int ballId = 100;
        var cart = new Cart { Id = cartId, Balls = new List<ShoppingCart.Domain.Models.Ball>() };

        _repositoryMock.Setup(r => r.FindById(cartId)).Returns(cart);

        // Act
        _cartService.RemoveProductFromCart(cartId, ballId);

        // Assert
        _repositoryMock.Verify(r => r.Update(It.IsAny<Cart>()), Times.Never);
    }

    [Fact]
    public void GetCart_ReturnsMappedCartDto_WhenCartExists()
    {
        // Arrange
        int cartId = 1;
        var cart = new Cart { Id = cartId, Balls = new List<ShoppingCart.Domain.Models.Ball>() };
        var cartDto = new CartDto { CartId = cartId };

        _repositoryMock.Setup(r => r.FindById(cartId)).Returns(cart);
        _mapperMock.Setup(m => m.Map<CartDto>(cart)).Returns(cartDto);

        // Act
        var result = _cartService.GetCart(cartId);

        // Assert
        Assert.Equal(cartDto, result);
    }

    [Fact]
    public void GetCart_ReturnsNull_WhenCartDoesNotExist()
    {
        // Arrange
        int cartId = 1;
        _repositoryMock.Setup(r => r.FindById(cartId)).Returns((Cart)null);

        // Act
        var result = _cartService.GetCart(cartId);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public void GetAllCarts_ReturnsMappedListOfCartDtos()
    {
        // Arrange
        var carts = new List<Cart> {
            new Cart { Id = 1 },
            new Cart { Id = 2 }
        };
        var cartsDto = new List<CartDto> {
            new CartDto { CartId = 1 },
            new CartDto { CartId = 2 }
        };

        _repositoryMock.Setup(r => r.GetAll()).Returns(carts);
        _mapperMock.Setup(m => m.Map<List<CartDto>>(carts)).Returns(cartsDto);

        // Act
        var result = _cartService.GetAllCarts();

        // Assert
        Assert.Equal(cartsDto, result);
    }
}
