using ShoppingCart.Domain.Models;
using ShoppingCart.Domain.ModelsDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCart.Domain.Interfaces
{
    public interface ICartReader
    {
        public CartDto GetCart(int cartId);
        public List<CartDto> GetAllCarts();
    }
}
