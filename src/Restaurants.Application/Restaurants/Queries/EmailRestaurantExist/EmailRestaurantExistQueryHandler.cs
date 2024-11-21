using MediatR;
using Microsoft.Extensions.Logging;
using Restaurants.Domain.Repositories;


namespace Restaurants.Application.Restaurants.Queries.EmailRestaurantExist
{
    public class EmailRestaurantExistQueryHandler(ILogger<EmailRestaurantExistQueryHandler> logger,
     IRestaurantsRepository restaurantsRepository) : IRequestHandler<EmailRestaurantExistQuery, bool>
    {
        public async Task<bool> Handle(EmailRestaurantExistQuery request, CancellationToken cancellationToken)
        {
            logger.LogInformation($"Deleting Restaurant with id : {request.Email}");
            var restaurant = await restaurantsRepository.GetByEmailAsync(request.Email);
            if (restaurant is null)
            {
                return false;
            }

            return true;
        }
    }
}
