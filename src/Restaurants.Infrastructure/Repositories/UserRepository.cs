using Microsoft.EntityFrameworkCore;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Repositories;
using Restaurants.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurants.Infrastructure.Repositories
{
    internal class UserRepository(RestaurantsDbContext context) : IUserRepository
    {
        

        public async Task<User> GetByIdAsync(string id)
        {
            return await context.Users.FindAsync(id);
        }

        public async Task<User> GetByEmailAsync(string email)
        {
            return await context.Users.FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<bool> AddAsync(User user)
        {   
            context.Users.Add(user);
            return await context.SaveChangesAsync() > 0;
        }

        public async Task UpdateAsync(User user)
        {
            context.Users.Update(user);
            await context.SaveChangesAsync();
        }

        public async Task<bool> DeleteAsync(User user)
        {
            context.Users.Remove(user);
            return await context.SaveChangesAsync() > 0;
        }

      
    }
}
