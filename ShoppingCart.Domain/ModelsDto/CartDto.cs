using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCart.Domain.ModelsDto
{
    public class CartDto
    {
        public int CartId { get; set; }
        public ICollection<BallDto> Balls { get; set; }
    }
}
