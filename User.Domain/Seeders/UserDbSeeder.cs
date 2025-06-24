using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using User.Domain.Models.Entities;

namespace User.Domain.Seeders
{
    public class UserDbSeeder : IUserDbSeeder
    {
        private readonly UserDbContext _dbContext;
        private readonly IPasswordHasher<Domain.Models.Entities.User> _passwordHasher;
        public UserDbSeeder(UserDbContext dbContext, IPasswordHasher<Domain.Models.Entities.User> passwordHasher)
        {
            _dbContext = dbContext;
            _passwordHasher = passwordHasher;
        }
        public void Seed()
        {
            if (!_dbContext.Roles.Any())
            {
                var roles = new List<Role>
                {
                    new Role { Name = "Admin" },
                    new Role { Name = "User" }
                };

                _dbContext.Roles.AddRange(roles);
                _dbContext.SaveChanges();
            }

            if (!_dbContext.Users.Any())
            {
                var admin = new User.Domain.Models.Entities.User
                {
                    Username = "admin",
                    Email = "admin@example.com",
                    RoleId = 1, // Administrator
                    IsActive = true
                };

                admin.PasswordHash = _passwordHasher.HashPassword(admin, "password");

                _dbContext.Users.Add(admin);
                _dbContext.SaveChanges();
            }
        }
    }
}
