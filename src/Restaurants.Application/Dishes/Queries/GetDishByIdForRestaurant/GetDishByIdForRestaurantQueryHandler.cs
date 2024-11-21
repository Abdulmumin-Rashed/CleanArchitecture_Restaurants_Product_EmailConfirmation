

using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Restaurants.Application.Dishes.Dtos;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Exceptions;
using Restaurants.Domain.Repositories;

namespace Restaurants.Application.Dishes.Queries.GetDishByIdForRestaurant;

public class GetDishByIdForRestaurantQueryHandler(ILogger<GetDishByIdForRestaurantQueryHandler> logger,
    IRestaurantsRepository restaurantsRepository, IMapper mapper) : IRequestHandler<GetDishByIdForRestaurantQuery, DishDto>
{
    public async Task<DishDto> Handle(GetDishByIdForRestaurantQuery request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Retrieving dish:{dishId}, for resturant with id:{RestaurantId}",
            request.dishId,
            request.restaurantId);
        var restaurant = await restaurantsRepository.GetByIdAsync(request.restaurantId);
        if(restaurant == null)
        {
            throw new NotFoundException(nameof(Restaurant),request.restaurantId.ToString());

        }

        var dish = restaurant.Dishes.FirstOrDefault(d => d.Id == request.dishId);
        if(dish == null)
        {
            throw new NotFoundException(nameof(Dish), request.dishId.ToString());
        }
        var result = mapper.Map<DishDto>(dish);
        return result;

    }
}
