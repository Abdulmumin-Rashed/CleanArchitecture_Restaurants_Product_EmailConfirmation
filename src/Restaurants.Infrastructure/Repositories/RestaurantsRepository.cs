using Microsoft.EntityFrameworkCore;
using Restaurants.Application.Common;
using Restaurants.Application.Restaurants.Dtos;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Repositories;
using Restaurants.Infrastructure.Persistence;
using System.Linq.Expressions;



namespace Restaurants.Infrastructure.Repositories
{
    internal class RestaurantsRepository(RestaurantsDbContext  dbContext) : IRestaurantsRepository
    {
        public async Task<int> Create(Restaurant entity)
        {
            dbContext.Restaurants.Add(entity);
            await dbContext.SaveChangesAsync();
            return entity.Id;
        }


       public async Task<IEnumerable<Restaurant>> GetAllAsync()
        {
            var restaurants = await dbContext.Restaurants
                 .Include(r => r.Dishes).ToListAsync();
            return restaurants;
        }

        public async Task<(IEnumerable<Restaurant>, int)> GetAllMatchingAsync(string? searchPhrase, int pageSize, int pageNumber,
            string? sortBy, SortDirection sortDirection)
        {
            var searchPhraseLower = searchPhrase?.ToLower();

            var baseQuery = dbContext.Restaurants
                .Where(r => searchPhraseLower == null || (r.Name.ToLower().Contains(searchPhraseLower)
                   || r.Description.ToLower().Contains(searchPhraseLower)));

            var totalCount = await baseQuery.CountAsync();

            if(sortBy != null)
            {
                var columnsSelector = new Dictionary<string, Expression<Func<Restaurant, object>>>
                {
                    {nameof(Restaurant.Name),n => n.Name },
                    {nameof(Restaurant.Category),n => n.Category },
                    {nameof(Restaurant.Description),n => n.Description },

                };
                var selectedColumn = columnsSelector[sortBy];

                baseQuery = sortDirection == SortDirection.Ascending
                    ?baseQuery.OrderBy(selectedColumn)
                    :baseQuery.OrderByDescending(selectedColumn);
               
            }
           

            int skip = pageSize * (pageNumber - 1);

            var restaurants = await baseQuery
                     .Skip(skip)
                     .Take(pageSize)
                 .ToListAsync();
            return (restaurants,totalCount);
        }
        public async Task<Restaurant?> GetByIdAsync(int id)
        {
            var restaurant = await dbContext.Restaurants
                //.Include(r =>r.Dishes)
                .FirstOrDefaultAsync(x => x.Id == id);
            return restaurant;
        }

        public bool EmailExists(string email)
        {

            var restaurant = dbContext.Restaurants.FirstOrDefault(u => u.ContactEmail == email);
            bool exists = restaurant != null;
            bool t = dbContext.Restaurants.Any(u => u.ContactEmail == email);
            return exists;
        }

        public async Task Delete(Restaurant entity)
        {
            dbContext.Remove(entity);
            await dbContext.SaveChangesAsync();
        }

        public async Task<Restaurant?> GetByEmailAsync(string email)
        {
            var restaurant = await dbContext.Restaurants
              .Include(r => r.Dishes).FirstOrDefaultAsync(x => x.ContactEmail == email);
            return restaurant;
        }

        public async Task<bool> EmailExistsAsync(string email)
        {
            return await dbContext.Restaurants.AnyAsync(r => r.ContactEmail == email);
        }

        public Task SaveChanges() => dbContext.SaveChangesAsync();

        
    }
}
