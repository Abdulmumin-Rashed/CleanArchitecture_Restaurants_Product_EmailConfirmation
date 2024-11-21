

using MediatR;
using Restaurants.Application.Models;

namespace Restaurants.Application.Auth.Commands.Email
{
    public class ConfirmEmailCommand : IRequest<Response>
    {
        public string Email { get; set; }
        public string Token { get; set; }
    }


}
