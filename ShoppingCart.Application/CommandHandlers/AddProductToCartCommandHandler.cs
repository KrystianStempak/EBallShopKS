using EShop.Domain.Models;
using MediatR;
using ShoppingCart.Domain.Command;
using ShoppingCart.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCart.Application.CommandHandlers
{
    public class AddProductToCartCommandHandler : IRequestHandler<AddProductToCartCommand>
    {
        private readonly ICartAdder _cartAdder;

        public AddProductToCartCommandHandler(ICartAdder cartAdder)
        {
            _cartAdder = cartAdder;
        }

        public Task Handle(AddProductToCartCommand command, CancellationToken cancellationToken)
        {
            var ball = new ShoppingCart.Domain.Models.Ball
            {
                BallId = command.BallId,
                Quantity = command.Quantity,  
                CartId = command.CartId
            };
            _cartAdder.AddProductToCart(command.CartId, ball);
            return Task.CompletedTask;
        }
    }
}
