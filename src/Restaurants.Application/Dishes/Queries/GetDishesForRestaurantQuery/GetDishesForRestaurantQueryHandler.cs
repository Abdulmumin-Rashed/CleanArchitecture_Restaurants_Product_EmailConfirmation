using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Restaurants.Application.Dishes.Dtos;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Exceptions;
using Restaurants.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurants.Application.Dishes.Queries.GetDishesForRestaurantQuery
{
    public class GetDishesForRestaurantQueryHandler(ILogger<GetDishesForRestaurantQuery> logger,
        IRestaurantsRepository restaurantsRepository, IMapper mapper) : IRequestHandler<GetDishesForRestaurantQuery, IEnumerable<DishDto>>
    {
        public async Task<IEnumerable<DishDto>> Handle(GetDishesForRestaurantQuery request, CancellationToken cancellationToken)
        {

            logger.LogInformation("Retrieving dishes for resturant with id:{RestaurantId}", request.RestaurantId);

            var restaurant = await restaurantsRepository.GetByIdAsync(request.RestaurantId);
            if(restaurant == null)
            {
                throw new NotFoundException(nameof(Restaurant),request.RestaurantId.ToString());
            }
            var result = mapper.Map<IEnumerable<DishDto>>(restaurant.Dishes);
            return result;
        }
    }
}
