using MediatR;
using Restaurants.Application.Models;


namespace Restaurants.Application.Auth.Commands.Register
{
    public class RegisterUserCommand : IRequest<Response<string>>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
