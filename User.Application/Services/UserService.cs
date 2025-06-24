using AutoMapper;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using User.Domain.Models.Entities;
using User.Domain.Models.Requests;
using User.Domain.Models.Response;
using User.Domain.Repositories;

namespace User.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IMapper _mapper;
        private readonly UserDbContext _db;
        private readonly IPasswordHasher<User.Domain.Models.Entities.User> _passwordHasher;

        public UserService(IMapper mapper, UserDbContext db, IPasswordHasher<User.Domain.Models.Entities.User> passwordHasher)
        {
            _mapper = mapper;
            _db = db;
            _passwordHasher = passwordHasher;
        }

        public UserResponseDTO GetUser(int userId)
        {
            var user = _db.Users.Find(userId);
            
            if (user == null)
                throw new Exception("Record not found");

            return _mapper.Map<UserResponseDTO>(user);
        }

        public void ResetPassword(ResetPasswordDTO dto)
        {
            var user = _db.Users.FirstOrDefault(u => u.Email == dto.Email);
            if (user == null)
                throw new Exception("User with given email not found");

            user.PasswordHash = _passwordHasher.HashPassword(user, dto.NewPassword);
            _db.SaveChanges();
        }

        public void EditUser(int userId, EditUserDTO dto)
        {
            var user = _db.Users.FirstOrDefault(u => u.Id == userId);
            if (user == null)
                throw new Exception("User not found");

            if (_db.Users.Any(u => u.Id != userId && u.Username == dto.Username))
                throw new Exception("Username is already taken");

            if (_db.Users.Any(u => u.Id != userId && u.Email == dto.Email))
                throw new Exception("Email is already taken");

            user.Username = dto.Username;
            user.Email = dto.Email;
            _db.SaveChanges();
        }

        public void RegisterUser(RegisterUserDTO dto)
        {
            if (_db.Users.Any(u => u.Username == dto.Username))
                throw new Exception("Username already taken");

            var user = _mapper.Map<User.Domain.Models.Entities.User>(dto);

            user.PasswordHash = _passwordHasher.HashPassword(user, dto.PasswordHash);

            
            user.RoleId = 2;

            _db.Users.Add(user);
            _db.SaveChanges();
        }
    }
}
