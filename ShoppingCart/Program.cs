using EShop.Domain;
using EShop.Domain.Models;
using MediatR;
using ShoppingCart.Application;
using ShoppingCart.Application.Services;
using ShoppingCart.Domain.Interfaces;
using ShoppingCart.Infrastructure.Repositories;
using ShoppingCart.Domain.Models;
using ShoppingCart.Domain.Seeders;
using ShoppingCart.Application.QueryHandlers;
using EShop.Domain.Seeders;
using ShoppingCart.Domain.Queries;
using Microsoft.EntityFrameworkCore;

namespace ShoppingCart
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(CartService).Assembly));
            builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(GetAllCartsQueryHandler).Assembly));
           
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddScoped<ICartRepository, EFCoreCartRepository>();
            builder.Services.AddScoped<ICartAdder, CartService>();
            builder.Services.AddScoped<ICartRemover, CartService>();
            builder.Services.AddScoped<ICartReader, CartService>();

            builder.Services.AddHttpClient<IBallInfoProvider, ApiBallInfoProvider>(client =>
            {
                client.BaseAddress = new Uri("https://localhost:5211"); // <-- Twój adres EBallShop API
            });

            builder.Services.AddDbContext<ShoppingCartDbContext>(options =>
                options.UseInMemoryDatabase("ShoppingCartDb"));
            builder.Services.AddScoped<IShoppingCartSeeder, ShoppingCartSeeder>();
            builder.Services.AddAutoMapper(typeof(MappingProfile).Assembly);

            var app = builder.Build();

            using (var scope = app.Services.CreateScope())
            {
                var seeder = scope.ServiceProvider.GetRequiredService<IShoppingCartSeeder>();
                seeder.Seed();
            }

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
