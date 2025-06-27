using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using EBallShop;
using EShop.Domain;
using Microsoft.AspNetCore.Hosting;
using EShop.Domain.Models;

public class CustomWebApplicationFactory<TProgram> : WebApplicationFactory<TProgram> where TProgram : class
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            // Usuwamy istniejący kontekst
            var descriptor = services.SingleOrDefault(
                d => d.ServiceType == typeof(DbContextOptions<BallDbContext>));

            if (descriptor != null)
                services.Remove(descriptor);

            // Dodajemy in-memory DB
            services.AddDbContext<BallDbContext>(options =>
            {
                options.UseInMemoryDatabase("InMemoryDbForTesting");
            });

            // Seedowanie danych (opcjonalnie)
            var sp = services.BuildServiceProvider();

            using var scope = sp.CreateScope();
            var scopedServices = scope.ServiceProvider;
            var db = scopedServices.GetRequiredService<BallDbContext>();

            db.Database.EnsureCreated();

            db.Balls.Add(new EShop.Domain.Models.Ball
            {
                Name = "SeedBall",
                Size = "Medium",
                Description = "For test",
                Category = new EShop.Domain.Models.Category { Name = "TestCategory" }
            });

            db.SaveChanges();
        });
    }
}
