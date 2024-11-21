

namespace Restaurants.Application.Dtos
{
    public class TokenResponse
    {
        public string? AccessToken { get; set; }
        public DateTime Expiration { get; set; }
        public string? RefreshToken { get; set; }
    }
}
