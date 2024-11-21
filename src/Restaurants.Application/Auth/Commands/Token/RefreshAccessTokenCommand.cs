

using MediatR;
using Restaurants.Application.Models;
using Restaurants.Domain.HelperModels;

namespace Restaurants.Application.Auth.Commands.Token
{
    public class RefreshAccessTokenCommand : IRequest<Response<AccessToken>>
    {
        public string? Token { get; set; }
        public string? RefreshToken { get; set; }

        // Constructor to initialize the properties
        public RefreshAccessTokenCommand(string token, string refreshToken)
        {
            Token = token;
            RefreshToken = refreshToken;
        }
    }

}
