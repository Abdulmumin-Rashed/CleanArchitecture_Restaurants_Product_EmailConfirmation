using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Restaurants.Domain.Repositories;

namespace Restaurants.Application.Restaurants.Queries.CheckEmailExistence
{
    internal class CheckEmailExistenceQueryHandler(ILogger<CheckEmailExistenceQueryHandler> logger,
        IMapper mapper,
        IRestaurantsRepository restaurantsRepository) : IRequestHandler<CheckEmailExistenceQuery, bool>
    {
        public async Task<bool> Handle(CheckEmailExistenceQuery request, CancellationToken cancellationToken)
        {
            var restaurant = await restaurantsRepository.EmailExistsAsync(request.Email);
            if(restaurant is false) return false;
            return true; // Return true if email exists
        }
    }
}
