using MediatR;
using Microsoft.AspNetCore.Identity;
using Restaurants.Application.Models;
using Restaurants.Domain.Entities;
using Restaurants.Domain.HelperModels;
using Restaurants.Domain.Interfaces;
using System.Security.Claims;


namespace Restaurants.Application.Auth.Commands.Login
{
    public class LoginCommandHandler : IRequestHandler<LoginUserCommand, Response<AccessToken>>
    {
        private readonly UserManager< User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly ITokenService _tokenService;

        public LoginCommandHandler(UserManager<User> userManager, SignInManager<User> signInManager, ITokenService tokenService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenService = tokenService;
        }

        public async Task<Response<AccessToken>> Handle(LoginUserCommand request, CancellationToken cancellationToken)
        {
            try
            {

                var user = await _userManager.FindByEmailAsync(request.Email);
                if (user != null)
                {
                    var isValid = await _signInManager.PasswordSignInAsync(user, request.Password, false, true);
                    if (!isValid.Succeeded)
                    {
                        var errorResponse = new Response<AccessToken>(false, "User login failed, password is not correct");
                        return errorResponse;
                    }

                    var roles = await _userManager.GetRolesAsync(user);
                    if (roles != null)
                    {
                        var allUserClaims = await _userManager.GetClaimsAsync(user);
                        foreach (var role in roles)
                        {
                            var oldCurrentRoleClaim = allUserClaims.FirstOrDefault(c => c.Type == ClaimTypes.Role);
                            var newCurrentRoleClaim = new Claim(ClaimTypes.Role, role);
                            if (oldCurrentRoleClaim == null)
                            {
                                await _userManager.AddClaimAsync(user, newCurrentRoleClaim);
                            }
                            else if (oldCurrentRoleClaim.Value != newCurrentRoleClaim.Value)
                            {
                                await _userManager.ReplaceClaimAsync(user, oldCurrentRoleClaim, newCurrentRoleClaim);
                            }
                        }
                        var token = await _tokenService.GenerateJwtToken(user);
                        var result = new Response<AccessToken>(true, "Success login", token);
                        return result;
                    }
                    var response = new Response<AccessToken>(false, "This user does not have any roles assigned");
                    return response;
                }
                else
                {
                    var response = new Response<AccessToken>(false, "This user does not exist");
                    return response;
                }
            }
            catch (Exception ex)
            {
                // Log the exception if necessary
                return new Response<AccessToken>(false, $"An error occurred: {ex.Message}");
            }
        }
    }
}
