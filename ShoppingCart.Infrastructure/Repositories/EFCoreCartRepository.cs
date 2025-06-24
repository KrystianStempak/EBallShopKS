using Microsoft.EntityFrameworkCore;
using ShoppingCart.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCart.Infrastructure.Repositories
{
    public class EFCoreCartRepository : ICartRepository
    {
        private readonly ShoppingCartDbContext _context;

        public EFCoreCartRepository(ShoppingCartDbContext context)
        {
            _context = context;
        }

        public Cart FindById(int id)
        {
            return _context.Carts
                .Include(c => c.Balls)
                .FirstOrDefault(c => c.Id == id);
        }

        //public void Add(Cart cart)
        //{
        //    if (_context.Carts.Any(c => c.Id == cart.Id))
        //    {
        //        _context.Carts.Update(cart);
        //    }
        //    else
        //    {
        //        _context.Carts.Add(cart);
        //    }

        //    _context.SaveChanges();
        //}

        public void Add(Cart cart)
        {
            var existingCart = _context.Carts
                .Include(c => c.Balls)  // jeśli chcesz aktualizować kolekcję piłek
                .FirstOrDefault(c => c.Id == cart.Id);

            if (existingCart != null)
            {
                // Aktualizuj właściwości existingCart na podstawie cart
                // np. existingCart.Name = cart.Name; itd.

                // Przykładowo aktualizacja kolekcji piłek:
                existingCart.Balls = cart.Balls;

                _context.Carts.Update(existingCart);
            }
            else
            {
                _context.Carts.Add(cart);
            }

            _context.SaveChanges();
        }

        public void Update(Cart cart)
        {
            _context.Carts.Update(cart);
            _context.SaveChanges();
        }

        public List<Cart> GetAll()
        {
            return _context.Carts
                .Include(c => c.Balls)
                .ToList();
        }
    }

}
