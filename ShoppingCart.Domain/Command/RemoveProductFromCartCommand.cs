﻿using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCart.Domain.Command
{
    public class RemoveProductFromCartCommand : IRequest
    {
        public int CartId { get; set; }
        public int ProductId { get; set; }
    }
}
