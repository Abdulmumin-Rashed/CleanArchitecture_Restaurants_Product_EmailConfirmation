

using MediatR;
using Restaurants.Application.Dishes.Dtos;

namespace Restaurants.Application.Dishes.Queries.GetDishesForRestaurantQuery;

public class GetDishesForRestaurantQuery(int restaurantId):IRequest<IEnumerable<DishDto>>
{
    public int RestaurantId { get; } = restaurantId;
}
