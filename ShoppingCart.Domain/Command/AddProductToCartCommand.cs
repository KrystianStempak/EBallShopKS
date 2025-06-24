using Azure.Core;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCart.Domain.Command
{
    public class AddProductToCartCommand : IRequest
    {
        public int CartId { get; set; }
        public int BallId { get; set; }
        public int Quantity { get; set; } =  1;
    }
}
