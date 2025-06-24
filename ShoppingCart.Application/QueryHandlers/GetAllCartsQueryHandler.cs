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
    public class GetAllCartsQueryHandler : IRequestHandler<GetAllCartsQuery, List<CartDto>>
    {
        private readonly ICartReader _cartReader;

        public GetAllCartsQueryHandler(ICartReader cartReader)
        {
            _cartReader = cartReader;
        }

        public Task<List<CartDto>> Handle(GetAllCartsQuery request, CancellationToken cancellationToken)
        {
            return Task.FromResult(_cartReader.GetAllCarts());
        }
    }
}
