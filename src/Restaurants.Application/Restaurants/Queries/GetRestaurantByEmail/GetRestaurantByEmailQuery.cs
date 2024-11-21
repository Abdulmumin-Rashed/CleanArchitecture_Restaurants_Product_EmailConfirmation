

using MediatR;
using Restaurants.Application.Restaurants.Dtos;

namespace Restaurants.Application.Restaurants.Queries.GetRestaurantByEmail
{
    public class GetRestaurantByEmailQuery(string email):IRequest<RestaurantDto?>
    {
        public string Email { get; } = email;
    }
}
