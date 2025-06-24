using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCart.Domain.Models
{
    public class Ball
    {
        public int Id { get; set; }
        public int BallId { get; set; }
        public int CartId { get; set; }  // FK
        public Cart Cart { get; set; }   // Navigation property
        public int Quantity { get; set; } = 1;

    }
}
