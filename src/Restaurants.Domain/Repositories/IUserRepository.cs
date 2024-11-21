

using Restaurants.Domain.Entities;

namespace Restaurants.Domain.Repositories
{
    public interface IUserRepository
    {
        Task<User> GetByIdAsync(string id);
        Task<User> GetByEmailAsync(string email);
        Task<bool> AddAsync(User user);
        Task UpdateAsync(User user);
        Task<bool> DeleteAsync(User user);
    }
}
