﻿

namespace Restaurants.Infrastructure.HelperModels
{
    public class AccessToken
    {
        public string? Token { get; set; }
        public DateTime Expiration { get; set; }
        public string? RefreshToken { get; set; }
    }
}
