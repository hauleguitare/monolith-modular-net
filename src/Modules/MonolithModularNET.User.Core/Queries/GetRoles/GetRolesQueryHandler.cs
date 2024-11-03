using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MonolithModularNET.Auth.Core;
using MonolithModularNET.Extensions.Shared.Cqrs;
using MonolithModularNET.Extensions.Shared.Models;

namespace MonolithModularNET.User.Core.Queries.GetRoles;

public class GetRolesQueryHandler(RoleManager<AuthRole> roleManager)
    : CqrsQueryHandler<GetRolesQuery, ICollection<RoleResponse>>
{
    private RoleManager<AuthRole> _roleManager = roleManager;

    public override async Task<ICollection<RoleResponse>> Handle(GetRolesQuery request, CancellationToken cancellationToken)
    {
        var roles = await _roleManager.Roles.AsNoTracking().OrderBy(e => e.Priority).ToListAsync(cancellationToken: cancellationToken);
        var roleResponses = new List<RoleResponse>();

        foreach (var role in roles)
        {
            var claims = await _roleManager.GetClaimsAsync(role);

            var permissions = claims.Where(e => e.Type == AuthClaimTypes.Permission).Select(e => e.Value).ToList();
            
            roleResponses.Add(new RoleResponse()
            {
                Id = role.Id,
                Name = role.Name,
                NormalizedName = role.NormalizedName,
                Priority = role.Priority,
                IsDefault = role.IsDefault,
                Permissions = permissions
            });
        }

        return roleResponses;
    }
}