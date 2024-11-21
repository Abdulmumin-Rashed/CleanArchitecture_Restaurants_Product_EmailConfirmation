

using Azure.Core;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Restaurants.Infrastructure.Services
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _configuration;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public TokenService(IConfiguration configuration, UserManager<User> userManager, SignInManager<User> signInManager,
            RoleManager<IdentityRole> roleManager)
        {
            _configuration = configuration;
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

        public async Task<Domain.HelperModels.AccessToken> GenerateJwtToken(User user)
        {

            try
            {
                var jwtTokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_configuration["JWT:Secret"]);
                var claims = await GetAllValidClaims(user);
                _ = int.TryParse(_configuration["JWT:TokenValidityInMinutes"], out int tokenValidityInMinutes);
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(claims),
                    Expires = DateTime.UtcNow.AddMinutes(tokenValidityInMinutes),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256),
                    Issuer = _configuration["JWT:ValidIssuer"],
                    Audience = _configuration["JWT:ValidAudience"]
                };
                var token = jwtTokenHandler.CreateToken(tokenDescriptor);
                var jwtToken = jwtTokenHandler.WriteToken(token);
                var refreshToken = GenerateRefreshToken();
                _ = int.TryParse(_configuration["JWT:RefreshTokenValidity"], out int refreshTokenValidity);
                var expiration = DateTime.UtcNow.AddMinutes(refreshTokenValidity);
                user.RefreshToken = refreshToken;
                user.RefreshTokenExpiry = expiration;
                await _userManager.UpdateAsync(user);
                return new Domain.HelperModels.AccessToken() { Token = jwtToken, RefreshToken = refreshToken, Expiration = expiration };
            }
            catch (Exception)
            {

                throw;
            }

            //var jwtTokenHandler = new JwtSecurityTokenHandler();
            //var key = Encoding.ASCII.GetBytes(_configuration["JWT:Secret"]);
            //var claims = await GetAllValidClaims(user);
            //_ = int.TryParse(_configuration["JWT:TokenValidityInMinutes"], out int tokenValidityInMinutes);
            //var tokenDescriptor = new SecurityTokenDescriptor
            //{
            //    Subject = new ClaimsIdentity(claims),
            //    Expires = DateTime.UtcNow.AddMinutes(tokenValidityInMinutes),
            //    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
            //    SecurityAlgorithms.HmacSha256),
            //    Issuer = _configuration["JWT:ValidIssuer"],
            //    Audience = _configuration["JWT:ValidAudience"]
            //};
            //var token = jwtTokenHandler.CreateToken(tokenDescriptor);
            //var jwtToken = jwtTokenHandler.WriteToken(token);
            //var refreshToken = GenerateRefreshToken();
            //_ = int.TryParse(_configuration["JWT:RefreshTokenValidity"], out int refreshTokenValidity);
            //var expiration = DateTime.UtcNow.AddMinutes(refreshTokenValidity);
            //user.RefreshToken = refreshToken;
            //user.RefreshTokenExpiry = expiration;
            //await _userManager.UpdateAsync(user);
            //return new Domain.HelperModels.AccessToken() { Token = jwtToken, RefreshToken = refreshToken, Expiration = expiration };

        }
        private async Task<List<Claim>> GetAllValidClaims(User user)
        {
            try
            {
                var claims = new List<Claim>()
                    {
                        new Claim(ClaimTypes.NameIdentifier, user.Id),
                        new Claim(ClaimTypes.Name, user.UserName),
                        new Claim(ClaimTypes.Email, user.Email),
                        new Claim("Nationality", user.Nationality),
                        new Claim("DateOfBirth", user.DateOfBirth.Value.ToString("yyyy-MM-dd")),
                        new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                      
                    };
                var userClaims = await _userManager.GetClaimsAsync(user);
                claims.AddRange(userClaims);
                var userRoles = await _userManager.GetRolesAsync(user);
                foreach (var userRole in userRoles)
                {
                    var role = await _roleManager.FindByNameAsync(userRole);
                    if (role != null)
                    {
                        claims.Add(new Claim("roles", userRole));
                        var roleClaims = await _roleManager.GetClaimsAsync(role);
                        claims.AddRange(roleClaims);
                    }

                }
                return claims;
            }
            catch (Exception)
            {

                throw;
            }
        }

        private static string GenerateRefreshToken()
        {
            var randomNumber = new byte[64];
            var range = RandomNumberGenerator.Create();
            range.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }

      

        public bool ValidateRefreshToken(string refreshToken, string storedRefreshToken)
        {
            throw new NotImplementedException();
        }
    }
}
