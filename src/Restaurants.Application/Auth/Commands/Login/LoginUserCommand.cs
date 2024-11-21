using MediatR;
using Restaurants.Application.Models;
using Restaurants.Domain.HelperModels;


namespace Restaurants.Application.Auth.Commands.Login
{
    public class LoginUserCommand : IRequest<Response<AccessToken>>
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
