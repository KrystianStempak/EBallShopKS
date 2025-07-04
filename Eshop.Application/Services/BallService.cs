﻿using AutoMapper;
using EShop.Domain.Models;
using EShop.Domain.ModelsDto;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Net.Http;

namespace EShop.Application.Services
{
    public class BallService : IBallService
    {
        private readonly BallDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly ILogger<BallService> _logger;
        private readonly HttpClient _httpClient;

        public BallService(HttpClient httpClient, IConfiguration config, BallDbContext dbContext, IMapper mapper, ILogger<BallService> logger)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri(config["BallService:BaseUrl"]);
            _dbContext = dbContext;
            _mapper = mapper;
            _logger = logger;
        }

        public bool Update(int id, UpdateBallDto dto)
        {
            var ball = _dbContext
                .Balls
                .FirstOrDefault(b => b.Id == id);

            if (ball is null) return false;

            ball.Name = dto.Name;
            ball.Size = dto.Size;
            ball.Description = dto.Description;

            _dbContext.SaveChanges();

            return true;
        }

        public bool Delete(int id)
        {

            var ball = _dbContext
                .Balls
                .FirstOrDefault(b => b.Id == id);

            if (ball is null) return false;

            _dbContext.Balls.Remove(ball);
            _dbContext.SaveChanges();

            return true;
        }

        public BallDto GetById(int id) 
        {
            var ball = _dbContext
                .Balls
                .Include(b => b.Category)
                .FirstOrDefault(b => b.Id == id);

            if(ball is null)
            {
                return null;
            }

            var result = _mapper.Map<BallDto>(ball);
            return result;
        }

        public IEnumerable<BallDto> GetAll()
        {
            var balls = _dbContext
                .Balls
                .Include(b => b.Category)
                .ToList();

            var ballsDtos = _mapper.Map<List<BallDto>>(balls);

            return ballsDtos;
        }

        public int Create(CreateBallDto dto)
        {
            var ball = _mapper.Map<Ball>(dto);

            // Ustaw domyślną kategorię, jeśli nie jest podana
            if (ball.CategoryId == 0)
            {
                ball.CategoryId = 1; // Id istniejącej kategorii domyślnej
            }

            _dbContext.Balls.Add(ball);
            _dbContext.SaveChanges();

            return ball.Id;
        }
    }
}
