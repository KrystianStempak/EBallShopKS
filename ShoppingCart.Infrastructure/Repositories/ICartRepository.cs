using ShoppingCart.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCart.Infrastructure.Repositories
{
    public interface ICartRepository
    {
        void Add(Cart cart);
        void Update(Cart cart);
        Cart FindById(int id);
        List<Cart> GetAll();
    }
}
