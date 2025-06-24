using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer;
using Microsoft.EntityFrameworkCore.Migrations;


namespace EShop.Domain.Models
{

    public class BallDbContext : DbContext
    {
        public BallDbContext(DbContextOptions<BallDbContext> options)
            : base(options)
        {
        }

        public DbSet<Ball> Balls { get; set; }
        public DbSet<Category> Categories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        { 

            modelBuilder.Entity<Category>()
                .HasIndex(c => c.Name)
                .IsUnique();
        }
    }
}
