

using Microsoft.EntityFrameworkCore;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Repositories;
using Restaurants.Infrastructure.Persistence;

namespace Restaurants.Infrastructure.Repositories;

internal class ProductRepository : IProductRepository
{
    private readonly RestaurantsDbContext _context;

    public ProductRepository(RestaurantsDbContext context)
    {
        _context = context;
    }

    public async Task<Product?> GetByIdAsync(int id)
    {
        var pro = await _context.Products.Include(p => p.Category).FirstOrDefaultAsync(p => p.Id == id);
        return pro;
    }
    
    public async Task<List<Product>> GetAllAsync() => await _context.Products.Include(p => p.Category).ToListAsync();
    public async Task<int> AddAsync(Product product)
    {
        _context.Products.AddAsync(product);
        await _context.SaveChangesAsync();
        return product.Id;

    }
       
    public Task UpdateAsync(Product? product)
    {
        if (product == null)
        {
            throw new ArgumentNullException(nameof(product), "Product cannot be null.");
        }

        _context.Products.Update(product);
        return Task.CompletedTask;
    }

    public async Task DeleteAsync(int id)
    {
        var product = await GetByIdAsync(id);
        if (product != null) _context.Products.Remove(product);
    }
}
