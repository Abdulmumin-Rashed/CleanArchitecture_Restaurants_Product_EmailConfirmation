
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Restaurants.Application.Users.Commands.AssignUserRole;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Exceptions;

namespace Restaurants.Application.Users.Commands.UnAssignUserRole;

public class UnAssignUserRoleCommandHandler(ILogger<UnAssignUserRoleCommandHandler> logger,
    RoleManager<IdentityRole> roleManager, UserManager<User> userManager) : IRequestHandler<UnAssignUserRoleCommand>
{
    public async Task Handle(UnAssignUserRoleCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("UnAssinging User Role: {@Request}", request);
        var user = await userManager.FindByEmailAsync(request.UserEmail)
            ?? throw new NotFoundException(nameof(User), request.UserEmail);

        var role = await roleManager.FindByNameAsync(request.RoleName)
            ?? throw new NotFoundException(nameof(IdentityRole), request.RoleName);
     //   await userManager2.RemoveFromRoleAsync(user, request.RoleName);
        await userManager.RemoveFromRoleAsync(user, role.Name!);
    }
}
