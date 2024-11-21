

using MediatR;
using Microsoft.AspNetCore.Identity;
using Restaurants.Application.Models;
using Restaurants.Domain.Entities;

namespace Restaurants.Application.Auth.Commands.Email
{

    public class ConfirmEmailCommandHandler : IRequestHandler<ConfirmEmailCommand, Response>
    {
        private readonly UserManager<User> _userManager;

        public ConfirmEmailCommandHandler(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public async Task<Response> Handle(ConfirmEmailCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
            {
                return new Response(false, "User not found.");
            }

            var result = await _userManager.ConfirmEmailAsync(user, request.Token);
            if (!result.Succeeded)
            {
                var errors = string.Join("; ", result.Errors.Select(e => e.Description));
                return new Response(false, $"Email confirmation failed: {errors}");
            }

            return new Response(true, "Email confirmed successfully.");
        }
    }

}
