using EShop.Domain.Models;
using ShoppingCart.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ball = ShoppingCart.Domain.Models.Ball;

namespace ShoppingCart.Domain.Seeders
{
    public class ShoppingCartSeeder : IShoppingCartSeeder
    {
        private readonly ShoppingCartDbContext _dbContext;
        public ShoppingCartSeeder(ShoppingCartDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public void Seed()
        {
            if (!_dbContext.Carts.Any())
            {
                var cart = new Cart
                {
                    Balls = new List<Ball>() // pusty koszyk
                };

                _dbContext.Carts.Add(cart);
                _dbContext.SaveChanges();
            }
        }
    }
}
