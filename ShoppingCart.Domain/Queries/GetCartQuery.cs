using MediatR;
using ShoppingCart.Domain.Models;
using ShoppingCart.Domain.ModelsDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCart.Domain.Queries
{
    public class GetCartQuery : IRequest<CartDto>
    {
        public int CartId { get; set; }
    }
}
