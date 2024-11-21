

using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Restaurants.Application.Restaurants.Dtos;
using Restaurants.Domain.Repositories;

namespace Restaurants.Application.Restaurants.Queries.GetRestaurantByEmail
{
    public class GetRestaurantByEmailQueryHandler(ILogger<GetRestaurantByEmailQueryHandler> logger,
         IMapper mapper, IRestaurantsRepository restaurantsRepository) : IRequestHandler<GetRestaurantByEmailQuery, RestaurantDto?>
    {
        public async Task<RestaurantDto?> Handle(GetRestaurantByEmailQuery request, CancellationToken cancellationToken)
        {
            logger.LogInformation($"Getting Restaurant {request.Email}");

            var restaurant = await restaurantsRepository.GetByEmailAsync(request.Email);
            var restaurantDto = mapper.Map<RestaurantDto?>(restaurant);
            return restaurantDto;
        }
    }
}
