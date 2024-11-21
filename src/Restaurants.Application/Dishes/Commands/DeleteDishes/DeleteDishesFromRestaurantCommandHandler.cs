using MediatR;
using Microsoft.Extensions.Logging;
using Restaurants.Domain.Constants;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Exceptions;
using Restaurants.Domain.Interfaces;
using Restaurants.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurants.Application.Dishes.Commands.DeleteDishes
{
    public class DeleteDishesFromRestaurantCommandHandler(ILogger<DeleteDishesFromRestaurantCommandHandler> logger,
        IRestaurantsRepository restaurantsRepository,
        IDishesRepository dishesRepository, IRestaurantAuthorizationService restaurantAuthorizationService) : IRequestHandler<DeleteDishesFromRestaurantCommand>
    {
        public async Task Handle(DeleteDishesFromRestaurantCommand request, CancellationToken cancellationToken)
        {
            logger.LogWarning("Removing all dished from restaurant: {RestaurantId}", request.restaurantId);
            var restaurant = await restaurantsRepository.GetByIdAsync(request.restaurantId);
            if (restaurant == null)
            {
                throw new NotFoundException(nameof(Restaurant), request.restaurantId.ToString());
            }
            if (!restaurantAuthorizationService.Authorize(restaurant, ResourceOperation.Delete))
            {
                throw new ForbidException("This Permission");
            }
            await dishesRepository.Delete(restaurant.Dishes);

        }
    }
}
