using AutoMapper;
using Microsoft.AspNetCore.Identity;
using MonolithModularNET.Auth.Core;
using MonolithModularNET.Extensions.Shared.Cqrs;
using MonolithModularNET.Extensions.Shared.Models;

namespace MonolithModularNET.User.Core.Queries.GetUserById;

public class GetUserByIdQueryHandler: CqrsQueryHandler<GetUserByIdQuery, UserResponse?>
{
    private readonly UserManager<AuthUser> _userManager;
    private readonly RoleManager<AuthRole> _roleManager;
    private readonly IMapper _mapper;

    public GetUserByIdQueryHandler(UserManager<AuthUser> userManager, IMapper mapper, RoleManager<AuthRole> roleManager)
    {
        _userManager = userManager;
        _mapper = mapper;
        _roleManager = roleManager;
    }

    public override async Task<UserResponse?> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByIdAsync(request.UserId);

        if (user is null)
        {
            return null;
        }
        
        var userResponse = _mapper.Map<UserResponse>(user);

        var userRoleNames = await _userManager.GetRolesAsync(user);

        foreach (var userRoleName in userRoleNames)
        {
            var role = await _roleManager.FindByNameAsync(userRoleName);

            if (role is null)
            {
                ArgumentNullException.ThrowIfNull(role);
            }

            var claims = await _roleManager.GetClaimsAsync(role);

            userResponse.Roles.Add(new RoleResponse()
            {
                Id = role.Id,
                Name = role.Name,
                NormalizedName = role.NormalizedName,
                IsDefault = role.IsDefault,
                Priority = role.Priority,
                Permissions = claims.Where(e => e.Type == AuthClaimTypes.Permission).Select(e => e.Value).ToList()
            });
        }


        return userResponse;
    }
}