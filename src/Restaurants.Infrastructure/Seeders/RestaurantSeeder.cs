using Microsoft.AspNetCore.Identity;
using Restaurants.Domain.Constants;
using Restaurants.Domain.Entities;
using Restaurants.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurants.Infrastructure.Seeders
{
    internal class RestaurantSeeder(RestaurantsDbContext dbContext) : IRestaurantSeeder
    {
        public async Task Seed()
        {
            if (await dbContext.Database.CanConnectAsync())
            {
                if (!dbContext.Restaurants.Any())
                {
                    var restaurants = GetRestaurants();
                    dbContext.Restaurants.AddRange(restaurants);
                    await dbContext.SaveChangesAsync();

                }
                if (!dbContext.Roles.Any())
                {
                    var roles = GetRoles();
                    dbContext.Roles.AddRange(roles);
                    await dbContext.SaveChangesAsync();
                }
            }
        }

        private IEnumerable<IdentityRole> GetRoles()
        {
            List<IdentityRole> roles =
                [
                    new(UserRoles.Admin)
                    {
                        NormalizedName = UserRoles.Admin.ToUpper()
                    },
                    new(UserRoles.User)
                    {
                        NormalizedName = UserRoles.User.ToUpper()
                    },
                    new(UserRoles.Owner)
                    {
                        NormalizedName = UserRoles.Owner.ToUpper()
                    },



                ];
            return roles;
        }

        private IEnumerable<Restaurant> GetRestaurants()
        {
            List<Restaurant> restaurants = [
                new()
                {
                    Name = "Gourmet Paradise",
                    Description = "A fine dining experience with gourmet meals.",
                    Category = "Fine Dining",
                    HasDelivery = true,
                    ContactEmail = "contact@gourmetparadise.com",
                    ContactNamber = "123-456-7890",
                    Dishes = [
                        new()
                        {
                            Name = "Truffle Pasta",
                            Description = "Pasta with a rich truffle sauce.",
                            Price = 35.99m,
                        },
                        new()
                        {
                            Name = "Margherita Pizza",
                            Description = "Classic Margherita pizza with fresh ingredients.",
                            Price = 12.99m,
                        },
                    ],
                    Address = new()
                    {
                        City = "San Francisco",
                        Street = "Market Street",
                        PostalCode = "94103"
                    }
                },
                new()
                {
                    Name = "Pizza Town",
                    Description = "The best pizza in town.",
                    Category = "Fast Food",
                    HasDelivery = true,
                    ContactEmail = "contact@pizzatown.com",
                    ContactNamber = "987-654-3210",
                    Address = new Address()
                    {
                        City = "Chicago",
                        Street = "Lake Street",
                        PostalCode = "60601"
                    }
                }

            ];
            return restaurants;
        }
    }
}
