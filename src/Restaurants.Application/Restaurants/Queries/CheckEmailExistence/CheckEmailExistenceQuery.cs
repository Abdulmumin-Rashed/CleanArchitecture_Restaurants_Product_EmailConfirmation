using MediatR;


namespace Restaurants.Application.Restaurants.Queries.CheckEmailExistence
{
    public class CheckEmailExistenceQuery(string email):IRequest<bool>
    {
        public string Email { get; } = email; 
    }
}
