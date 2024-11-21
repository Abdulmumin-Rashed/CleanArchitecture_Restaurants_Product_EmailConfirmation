using Restaurants.Application.Common;
using Restaurants.Domain.Entities;

namespace Restaurants.Domain.Repositories
{
    public interface IRestaurantsRepository
    {
        Task<(IEnumerable<Restaurant>,int)> GetAllMatchingAsync(string? searchPhrase, int pageSize, int pageNumber,
            string? sortBy,SortDirection sortDirection);
        Task<IEnumerable<Restaurant>> GetAllAsync();
        Task<Restaurant?> GetByIdAsync(int id);
        Task<int>Create(Restaurant entity);
        bool EmailExists(string email);
        Task<bool> EmailExistsAsync(string email);
        Task Delete(Restaurant entity);
        Task<Restaurant?> GetByEmailAsync(string email);
        Task SaveChanges();


    }
}
