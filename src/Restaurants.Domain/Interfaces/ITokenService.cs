
using Restaurants.Domain.Entities;
using Restaurants.Domain.HelperModels;

namespace Restaurants.Domain.Interfaces
{
    public interface ITokenService
    {
        Task<AccessToken> GenerateJwtToken(User user);

        //  string GenerateRefreshToken();
        bool ValidateRefreshToken(string refreshToken, string storedRefreshToken);
    }
}
