
using Microsoft.EntityFrameworkCore;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Repositories;
using Restaurants.Infrastructure.Persistence;

namespace Restaurants.Infrastructure.Repositories;

internal class CategoryRepository(RestaurantsDbContext dbContext) : ICategoryRepository
{
    public async Task<Category?> GetByIdAsync(int id)
    {
       var cat = await dbContext.Categories.FirstOrDefaultAsync(c => c.Id == id);
        return cat;


    }
    public bool Exists(int categoryId) // Synchronous version
    {
        return dbContext.Categories.Any(c => c.Id == categoryId);
    }
    public async Task<bool> ExistsAsync(int categoryId)
    {
        return await dbContext.Categories.AnyAsync(c => c.Id == categoryId);
    }
    public async Task<List<Category>> GetAllAsync()
    {
        var cats = await dbContext.Categories.ToListAsync();
        return cats;
    } 
      
    public async Task AddAsync(Category category)
    {
        await dbContext.Categories.AddAsync(category);
        await dbContext.SaveChangesAsync();
    } 
    public async Task UpdateAsync(Category category)
    {
        dbContext.Categories.Update(category);
        await dbContext.SaveChangesAsync();

    }
    public async Task DeleteAsync(int id)
    {
        var category = await GetByIdAsync(id);
        if (category != null) dbContext.Categories.Remove(category);
    }
}
