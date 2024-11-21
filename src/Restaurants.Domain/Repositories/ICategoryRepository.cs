
using Restaurants.Domain.Entities;

namespace Restaurants.Domain.Repositories;

public interface ICategoryRepository
{
    Task<Category?> GetByIdAsync(int id);
    Task<List<Category>> GetAllAsync();
    Task AddAsync(Category category);
    Task UpdateAsync(Category category);
    Task DeleteAsync(int id);
    Task<bool> ExistsAsync(int categoryId);
    bool Exists(int categoryId); // Synchronous version
}
