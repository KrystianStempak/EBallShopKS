using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Authentication;
using System.Text;
using System.Threading.Tasks;
using User.Domain.Exceptions;
using User.Domain.Models.Entities;

namespace User.Application.Services
{
    public class LoginService : ILoginService
    {
        private readonly UserDbContext _dbContext;
        private readonly IPasswordHasher<Domain.Models.Entities.User> _passwordHasher;
        protected IJwtTokenService _jwtTokenService;
        protected Queue<int> _userLoggedIdsQueue;

        public LoginService(IJwtTokenService jwtTokenService, UserDbContext dbContext, IPasswordHasher<Domain.Models.Entities.User> passwordHasher)
        {
            _jwtTokenService = jwtTokenService;
            _userLoggedIdsQueue = new Queue<int>();
            _dbContext = dbContext;
            _passwordHasher = passwordHasher;
        }

        public string Login(string username, string password)
        {
            var user = _dbContext.Users
                .Include(u => u.Role)
                .FirstOrDefault(u => u.Username == username);

            if (user == null)
                throw new InvalidCredentialsException();


            var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, password);
            if (result == PasswordVerificationResult.Failed)
                throw new InvalidCredentialsException();

            var roles = user.Role != null ? new List<string> { user.Role.Name } : new List<string>(); var token = _jwtTokenService.GenerateToken(user.Id, roles);
            _userLoggedIdsQueue.Enqueue(user.Id);

            return token;

        }
    }
}
