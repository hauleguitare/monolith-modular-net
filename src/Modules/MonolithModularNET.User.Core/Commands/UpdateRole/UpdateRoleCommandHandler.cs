using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Identity;
using MonolithModularNET.Auth.Core;
using MonolithModularNET.Extensions.Shared.Cqrs;
using MonolithModularNET.Extensions.Shared.Models;
using MonolithModularNET.User.Core.Queries.GetRoleById;

namespace MonolithModularNET.User.Core.Commands.UpdateRole;

public class UpdateRoleCommandHandler(RoleManager<AuthRole> roleManager, ISender sender)
    : CqrsCommandHandler<UpdateRoleCommand, RoleResponse?>
{

    public override async Task<CqrsResult<RoleResponse?>> Handle(UpdateRoleCommand request, CancellationToken cancellationToken)
    {
        var describer = new UserErrorDescriber();
        var role = await roleManager.FindByNameAsync(request.Name);

        if (role is null)
        {
            return CqrsResult<RoleResponse?>.Failure(describer.RoleNotFound());
        }

        role.Name = request.Name;
        role.NormalizedName = request.Name.ToUpper();
        role.Priority = request.Priority;
        role.IsDefault = request.IsDefault;

        var roleClaims = await roleManager.GetClaimsAsync(role);
        var permissionClaims = roleClaims.Where(e => e.Type == AuthClaimTypes.Permission);


        foreach (var claim in permissionClaims)
        {
            await roleManager.RemoveClaimAsync(role ,claim);
        }


        foreach (var permission in request.Permissions)
        {
            await roleManager.AddClaimAsync(role, new Claim(AuthClaimTypes.Permission, permission));
        }

        var result = await sender.Send(new GetRoleByIdQuery(role.Id), cancellationToken);

        return CqrsResult<RoleResponse?>.Succeed(result);
    }
}