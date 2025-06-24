using EShop.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCart.Domain.Models
{
    public class Cart
    {
        public int Id { get; set; }
        public ICollection<Ball> Balls { get; set; } = new List<Ball>();
    }
}
