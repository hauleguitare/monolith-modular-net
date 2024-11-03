using Microsoft.AspNetCore.Identity;
using MonolithModularNET.Auth.Core;
using MonolithModularNET.Extensions.Shared.Cqrs;
using MonolithModularNET.Extensions.Shared.Models;

namespace MonolithModularNET.User.Core.Queries.GetRoleById;

public class GetRoleByIdQueryHandler(RoleManager<AuthRole> roleManager)
    : CqrsQueryHandler<GetRoleByIdQuery, RoleResponse?>
{
    public override async Task<RoleResponse?> Handle(GetRoleByIdQuery request, CancellationToken cancellationToken)
    {
        var role = await roleManager.FindByIdAsync(request.RoleId);

        if (role is null)
        {
            return null;
        }
        
        
        var claims = await roleManager.GetClaimsAsync(role);

        var permissions = claims.Where(e => e.Type == AuthClaimTypes.Permission).Select(e => e.Value).ToList();

        var roleResponse = new RoleResponse()
        {
            Id = role.Id,
            Name = role.Name,
            NormalizedName = role.NormalizedName,
            Priority = role.Priority,
            IsDefault = role.IsDefault,
            Permissions = permissions
        };

        return roleResponse;
    }
}