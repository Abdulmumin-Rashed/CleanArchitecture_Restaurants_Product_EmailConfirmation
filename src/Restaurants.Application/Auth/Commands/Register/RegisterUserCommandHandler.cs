using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Restaurants.Application.Models;
using Restaurants.Domain.Constants;
using Restaurants.Domain.Entities;
using Restaurants.Domain.HelperModels;
using Restaurants.Domain.Interfaces;
using System.Security.Claims;


namespace Restaurants.Application.Auth.Commands.Register
{

    public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, Response<string>>
    {
        private readonly UserManager<User> _userManager;
        private readonly IEmailService _emailService;

        public RegisterUserCommandHandler(UserManager<User> userManager, IEmailService emailService)
        {
            _userManager = userManager;
            _emailService = emailService;
        }

        public async Task<Response<string>> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            var userExist = await _userManager.FindByEmailAsync(request.Email);
            if (userExist != null)
            {
                return new Response<string>(false, "User already exists.");
            }

            var user = new User
            {
                Email = request.Email,
                UserName = request.Email,
                FirstName = request.FirstName,
                LastName = request.LastName,
                SecurityStamp = Guid.NewGuid().ToString()
            };

            var isCreated = await _userManager.CreateAsync(user, request.Password);
            if (!isCreated.Succeeded)
            {
                var errors = string.Join("; ", isCreated.Errors.Select(e => e.Description));
                return new Response<string>(false, $"User registration failed: {errors}");
            }

            await _userManager.AddToRoleAsync(user, UserRoles.User.ToString());
            await _userManager.AddClaimAsync(user, new Claim(ClaimTypes.Role, UserRoles.User.ToString()));

            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);

            // Generate confirmation link
            var confirmationLink = $"https://localhost:7181/api/User/confirm-email?token={Uri.EscapeDataString(token)}&email={Uri.EscapeDataString(user.Email)}";
            var messageBody = $"<a href=\"{confirmationLink}\" style=\"color: blue;\">Click here to confirm your email</a>";
            var message = new Message(new[] { user.Email }, "Confirm Your Email", messageBody);


            // Send email
            _emailService.SendEmail(message, user.Email);

            return new Response<string>(true, $"User registered successfully. A confirmation email has been sent to {user.Email}.", token);
        }
    }

}
