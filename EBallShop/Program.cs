using AutoMapper;
using EBallShop.Models;
using EBallShop.Services;
using Microsoft.Extensions.DependencyInjection;
using NLog;
using NLog.Web;
using System.Runtime.CompilerServices;

namespace EBallShop
{
    public class Program
    {
        public static void Main(string[] args)
        {
           
            var logger = LogManager.Setup().LoadConfigurationFromFile("nlog.config").GetCurrentClassLogger();
            try
            {
                logger.Debug("Init main");

                var builder = WebApplication.CreateBuilder(args);

                // Konfiguracja NLog jako loggera
                builder.Logging.ClearProviders();
                builder.Logging.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);
                builder.Host.UseNLog();

                // Add services to the container.
                builder.Services.AddControllers();
                // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
                builder.Services.AddEndpointsApiExplorer();
                builder.Services.AddSwaggerGen();

                builder.Services.AddDbContext<BallDbContext>();
                builder.Services.AddScoped<IBallSeeder, BallSeeder>();
                builder.Services.AddAutoMapper(typeof(Program).Assembly);
                builder.Services.AddScoped<IBallService, BallService>();

                var app = builder.Build();

                using (var scope = app.Services.CreateScope())
                {
                    var seeder = scope.ServiceProvider.GetRequiredService<IBallSeeder>();
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
            catch (Exception ex)
            {
                logger.Error(ex, "Application stopped because of an exception");
                throw;
            }
            finally
            {
                LogManager.Shutdown();
            }
           
        }

    }
}