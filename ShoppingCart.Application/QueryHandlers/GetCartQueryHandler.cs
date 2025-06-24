using MediatR;
using ShoppingCart.Domain.Interfaces;
using ShoppingCart.Domain.Models;
using ShoppingCart.Domain.ModelsDto;
using ShoppingCart.Domain.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCart.Application.QueryHandlers
{
    public class GetCartQueryHandler : IRequestHandler<GetCartQuery, CartDto>
    {
        private readonly ICartReader _cartReader;

        public GetCartQueryHandler(ICartReader cartReader)
        {
            _cartReader = cartReader;
        }

        public Task<CartDto> Handle(GetCartQuery query, CancellationToken cancellationToken)
        {
            return Task.FromResult(_cartReader.GetCart(query.CartId));
        }
    }
}
