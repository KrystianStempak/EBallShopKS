using Microsoft.EntityFrameworkCore;

namespace EBallShop.Models
{
    public class BallDbContext : DbContext
    {
        public DbSet<Ball> Balls { get; set; }
        public DbSet<Category> Categories { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            // Połączenie z bazą danych
            options.UseSqlServer("Server=DESKTOP-S77TFGV;Database=MyDatabase;Trusted_Connection=True;TrustServerCertificate=True;");
        }
    }
}
