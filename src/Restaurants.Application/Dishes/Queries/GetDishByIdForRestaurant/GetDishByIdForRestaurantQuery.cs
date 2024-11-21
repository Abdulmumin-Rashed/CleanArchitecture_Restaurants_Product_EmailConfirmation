

using MediatR;
using Restaurants.Application.Dishes.Dtos;

namespace Restaurants.Application.Dishes.Queries.GetDishByIdForRestaurant;

public class GetDishByIdForRestaurantQuery(int restaurantId, int dishId):IRequest<DishDto>
{
    public int restaurantId { get; } = restaurantId;
    public int dishId { get;} = dishId;
}
