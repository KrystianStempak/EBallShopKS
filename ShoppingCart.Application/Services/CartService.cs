using ShoppingCart.Domain.Interfaces;
using EShop.Domain.Models;
using ShoppingCart.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShoppingCart.Domain.Models;
using AutoMapper;
using ShoppingCart.Domain.ModelsDto;

namespace ShoppingCart.Application.Services
{

    public class CartService : ICartAdder, ICartRemover, ICartReader
    {
        private readonly ICartRepository _repository;
        private readonly IBallInfoProvider _ballInfoProvider;
        private readonly IMapper _mapper;

        public CartService(ICartRepository repository, IBallInfoProvider ballInfoProvider, IMapper mapper)
        {
            _repository = repository;
            _ballInfoProvider = ballInfoProvider;
            _mapper = mapper;
        }

        public void AddProductToCart(int cartId, ShoppingCart.Domain.Models.Ball ball)
        {
            var externalBall = _ballInfoProvider.GetBallByIdAsync(ball.BallId);

            if (externalBall == null)
            {
                throw new Exception($"Ball with ID {ball.BallId} does not exist in EBallShop.");
            }

            var cart = _repository.FindById(cartId) ?? new Cart { Id = cartId, Balls = new List<ShoppingCart.Domain.Models.Ball>() };

            var existingBall = cart.Balls.FirstOrDefault(b => b.BallId == ball.BallId);

            if (existingBall != null)
            {
                existingBall.Quantity += ball.Quantity;
            }
            else
            {
                cart.Balls.Add(ball);
            }

            _repository.Add(cart);
        }

        public void RemoveProductFromCart(int cartId, int ballId)
        {
            var cart = _repository.FindById(cartId);
            if (cart != null)
            {
                var ballToRemove = cart.Balls.FirstOrDefault(b => b.BallId == ballId);
                if (ballToRemove != null)
                {
                    cart.Balls.Remove(ballToRemove);
                    _repository.Update(cart);
                }
            }
        }

        public CartDto GetCart(int cartId)
        {
            var cart = _repository.FindById(cartId);

            if (cart == null)
                return null;

            // mapujemy encję Cart na CartDto
            var cartDto = _mapper.Map<CartDto>(cart);
            return cartDto;
        }

        public List<CartDto> GetAllCarts()
        {
            var carts = _repository.GetAll();
            var cartsDto = _mapper.Map<List<CartDto>>(carts);
            return cartsDto;
        }
    }
}
