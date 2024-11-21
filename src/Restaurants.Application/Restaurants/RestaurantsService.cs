using AutoMapper;
using Microsoft.Extensions.Logging;
using Restaurants.Application.Restaurants.Dtos;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurants.Application.Restaurants
{
    //internal class RestaurantsService(IRestaurantsRepository restaurantsRepository,
    //    ILogger<RestaurantsService> logger, IMapper mapper) : IRestaurantsService
    //{


    //    public bool EmailExists(string email)
    //    {
    //        logger.LogInformation("Email Existing");
    //        bool t = restaurantsRepository.EmailExists(email);
    //        return t;
    //    }

    //    public async Task<IEnumerable<RestaurantDto>> GetAllRestaurants()
    //    {

    //    }

    //    public async Task<RestaurantDto?> GetById(int id)
    //    {
    //        logger.LogInformation($"Getting Restaurant {id}");
    //        var restaurant = await restaurantsRepository.GetByIdAsync(id);
    //        var restaurantDto = mapper.Map<RestaurantDto?>(restaurant);
    //        return restaurantDto;

    //    }
    //}
}
