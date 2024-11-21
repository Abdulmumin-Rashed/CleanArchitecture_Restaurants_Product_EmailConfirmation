

using MediatR;
using Microsoft.AspNetCore.Identity;
using Restaurants.Application.Models;
using Restaurants.Domain.Entities;
using Restaurants.Domain.HelperModels;
using Restaurants.Domain.Interfaces;
using System.IdentityModel.Tokens.Jwt;

namespace Restaurants.Application.Auth.Commands.Token
{

    public class RefreshAccessTokenCommandHandler : IRequestHandler<RefreshAccessTokenCommand, Response<AccessToken>>
    {
        private readonly UserManager<User> _userManager;
        private readonly ITokenService _tokenService;

        public RefreshAccessTokenCommandHandler(UserManager<User> userManager, ITokenService tokenService)
        {
            _userManager = userManager;
            _tokenService = tokenService;
        }

        public async Task<Response<AccessToken>> Handle(RefreshAccessTokenCommand request, CancellationToken cancellationToken)
        {
            var accessToken = request.Token;
            var refreshToken = request.RefreshToken;

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.ReadJwtToken(accessToken);

            var userName = token.Subject;
            if (string.IsNullOrEmpty(userName))
            {
                return new Response<AccessToken>(false, "Invalid token");
            }

            var user = await _userManager.FindByNameAsync(userName);
            if (user == null)
            {
                return new Response<AccessToken>(false, $"User '{userName}' not found");
            }

            // Check if the refresh token is valid
            var refreshTokenExpiry = user.RefreshTokenExpiry;
            if (!refreshTokenExpiry.HasValue)
            {
                return new Response<AccessToken>(false, "Refresh Token is invalid");
            }

            if (refreshToken != user.RefreshToken)
            {
                return new Response<AccessToken>(false, "Refresh tokens do not match");
            }

            if (DateTime.UtcNow > refreshTokenExpiry.Value)
            {
                return new Response<AccessToken>(false, "Refresh Token is expired");
            }

            // Generate a new JWT token
            var newAccessToken = await _tokenService.GenerateJwtToken(user);
            return new Response<AccessToken>(true, "Token is refreshed successfully", newAccessToken);
        }
    }
}
