using Microsoft.AspNetCore.Identity;
using User.Domain.Models.Entities;

namespace User.Domain.Seeders
{
    public interface IUserDbSeeder
    {
        public void Seed();
    }
}