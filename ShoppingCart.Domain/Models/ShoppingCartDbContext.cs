using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCart.Domain.Models
{
    public class ShoppingCartDbContext : DbContext
    {
        public ShoppingCartDbContext(DbContextOptions<ShoppingCartDbContext> options)
        : base(options)
        {
        }

        public DbSet<Cart> Carts { get; set; }
        public DbSet<Ball> Balls { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Cart>()
                .HasKey(c => c.Id);

            modelBuilder.Entity<Cart>()
                .HasMany(c => c.Balls)
                .WithOne(b => b.Cart)
                .HasForeignKey(b => b.CartId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Ball>()
                .HasKey(b => b.Id);
        }
    }
}
