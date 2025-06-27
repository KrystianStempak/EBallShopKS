using AutoMapper;
using EShop.Application.Services;
using EShop.Domain.Models;
using EShop.Domain.ModelsDto;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using System;
using System.Collections.Generic;
using System.Net.Http;
using Xunit;

namespace EShop.Application.Tests
{
    public class BallServiceTests
    {
        private BallDbContext GetInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<BallDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            return new BallDbContext(options);
        }

        private IMapper GetMapperMock()
        {
            var mapperMock = new Mock<IMapper>();

            mapperMock.Setup(m => m.Map<BallDto>(It.IsAny<Ball>()))
                .Returns((Ball src) => new BallDto
                {
                    Id = src.Id,
                    Name = src.Name,
                    Size = src.Size,             // string
                    Description = src.Description
                });

            mapperMock.Setup(m => m.Map<List<BallDto>>(It.IsAny<List<Ball>>()))
                .Returns((List<Ball> src) =>
                {
                    var list = new List<BallDto>();
                    foreach (var ball in src)
                    {
                        list.Add(new BallDto
                        {
                            Id = ball.Id,
                            Name = ball.Name,
                            Size = ball.Size,         // string
                            Description = ball.Description
                        });
                    }
                    return list;
                });

            mapperMock.Setup(m => m.Map<Ball>(It.IsAny<CreateBallDto>()))
                .Returns((CreateBallDto src) => new Ball
                {
                    Name = src.Name,
                    Size = src.Size,             // string
                    Description = src.Description
                });

            return mapperMock.Object;
        }

        private IConfiguration GetConfigurationMock()
        {
            var configMock = new Mock<IConfiguration>();
            configMock.Setup(c => c["BallService:BaseUrl"]).Returns("http://localhost");
            return configMock.Object;
        }

        private HttpClient GetHttpClient()
        {
            return new HttpClient();
        }

        [Fact]
        public void Create_AddsBallAndReturnsId()
        {
            // Arrange
            var dbContext = GetInMemoryDbContext();
            var mapper = GetMapperMock();
            var logger = NullLogger<BallService>.Instance;
            var config = GetConfigurationMock();
            var httpClient = GetHttpClient();

            var service = new BallService(httpClient, config, dbContext, mapper, logger);

            var dto = new CreateBallDto
            {
                Name = "Test Ball",
                Size = "Large",               // string
                Description = "Test description"
            };

            // Act
            var id = service.Create(dto);

            // Assert
            var createdBall = dbContext.Balls.Find(id);
            Assert.NotNull(createdBall);
            Assert.Equal("Test Ball", createdBall.Name);
            Assert.Equal("Large", createdBall.Size);   // string
            Assert.Equal("Test description", createdBall.Description);
        }




        [Fact]
        public void GetById_ReturnsNull_WhenBallDoesNotExist()
        {
            // Arrange
            var dbContext = GetInMemoryDbContext();
            var mapper = GetMapperMock();
            var logger = NullLogger<BallService>.Instance;
            var config = GetConfigurationMock();
            var httpClient = GetHttpClient();

            var service = new BallService(httpClient, config, dbContext, mapper, logger);

            // Act
            var result = service.GetById(999);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void GetAll_ReturnsAllBalls_WithCategories()
        {
            // Konfiguracja in-memory bazy danych
            var options = new DbContextOptionsBuilder<BallDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDB_GetAll_WithCategories")
                .Options;

            using (var dbContext = new BallDbContext(options))
            {
                // Tworzymy kategorie
                var category1 = new Category { Id = 1, Name = "Category1" };
                var category2 = new Category { Id = 2, Name = "Category2" };

                // Tworzymy piłki, ustawiając referencję Category i CategoryId
                var ball1 = new Ball
                {
                    Id = 1,
                    Name = "Ball1",
                    Size = "Small",
                    Description = "Desc1",
                    CategoryId = 1,
                    Category = category1
                };
                var ball2 = new Ball
                {
                    Id = 2,
                    Name = "Ball2",
                    Size = "Large",
                    Description = "Desc2",
                    CategoryId = 2,
                    Category = category2
                };

                // Dodajemy kategorie i piłki do DbContext
                dbContext.Categories.AddRange(category1, category2);
                dbContext.Balls.AddRange(ball1, ball2);
                dbContext.SaveChanges();

                // Konfiguracja AutoMappera (przykład, zrób to tak jak w projekcie)
                var mapperConfig = new MapperConfiguration(cfg =>
                {
                    cfg.CreateMap<Ball, BallDto>()
                       .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.Name));
                    cfg.CreateMap<CreateBallDto, Ball>();
                    cfg.CreateMap<UpdateBallDto, Ball>();
                });
                var mapper = mapperConfig.CreateMapper();

                // Logger Null
                var logger = NullLogger<BallService>.Instance;

                // HttpClient i konfiguracja (minimalna)
                var config = new ConfigurationBuilder()
                    .AddInMemoryCollection(new Dictionary<string, string> { ["BallService:BaseUrl"] = "http://localhost" })
                    .Build();
                var httpClient = new HttpClient { BaseAddress = new Uri(config["BallService:BaseUrl"]) };

                // Utworzenie serwisu
                var service = new BallService(httpClient, config, dbContext, mapper, logger);

                // Wywołanie metody GetAll()
                var result = service.GetAll().ToList();

                // Assercje
                Assert.Equal(2, result.Count);

                Assert.Contains(result, b =>
                    b.Name == "Ball1" &&
                    b.Description == "Desc1" &&
                    b.Size == "Small" &&
                    b.CategoryId == 1 &&
                    b.CategoryName == "Category1");

                Assert.Contains(result, b =>
                    b.Name == "Ball2" &&
                    b.Description == "Desc2" &&
                    b.Size == "Large" &&
                    b.CategoryId == 2 &&
                    b.CategoryName == "Category2");
            }
        }






        [Fact]
        public void Update_ReturnsFalse_WhenBallDoesNotExist()
        {
            // Arrange
            var dbContext = GetInMemoryDbContext();
            var mapper = GetMapperMock();
            var logger = NullLogger<BallService>.Instance;
            var config = GetConfigurationMock();
            var httpClient = GetHttpClient();

            var service = new BallService(httpClient, config, dbContext, mapper, logger);

            var updateDto = new UpdateBallDto
            {
                Name = "NewName",
                Size = "Huge",                // string
                Description = "NewDesc"
            };

            // Act
            var result = service.Update(999, updateDto);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void Delete_ReturnsTrueAndRemovesBall_WhenBallExists()
        {
            // Arrange
            var dbContext = GetInMemoryDbContext();
            dbContext.Balls.Add(new Ball { Id = 1, Name = "ToDelete", Size = "Medium", Description = "Desc" }); // string
            dbContext.SaveChanges();

            var mapper = GetMapperMock();
            var logger = NullLogger<BallService>.Instance;
            var config = GetConfigurationMock();
            var httpClient = GetHttpClient();

            var service = new BallService(httpClient, config, dbContext, mapper, logger);

            // Act
            var result = service.Delete(1);

            // Assert
            Assert.True(result);
            Assert.Null(dbContext.Balls.Find(1));
        }

        [Fact]
        public void Delete_ReturnsFalse_WhenBallDoesNotExist()
        {
            // Arrange
            var dbContext = GetInMemoryDbContext();
            var mapper = GetMapperMock();
            var logger = NullLogger<BallService>.Instance;
            var config = GetConfigurationMock();
            var httpClient = GetHttpClient();

            var service = new BallService(httpClient, config, dbContext, mapper, logger);

            // Act
            var result = service.Delete(999);

            // Assert
            Assert.False(result);
        }
    }
}
