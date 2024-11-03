using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MonolithModularNET.Auth.Core;
using MonolithModularNET.Extensions.Shared.Cqrs;
using MonolithModularNET.Extensions.Shared.Models;

namespace MonolithModularNET.User.Core.Queries.GetUsers;

public class GetUsersQueryHandler: CqrsQueryHandler<GetUsersQuery, ICollection<UserResponse>>
{
    private UserManager<AuthUser> _userManager;
    private RoleManager<AuthRole> _roleManager;
    private IMapper _mapper;

    public GetUsersQueryHandler(UserManager<AuthUser> userManager, IMapper mapper, RoleManager<AuthRole> roleManager)
    {
        _userManager = userManager;
        _mapper = mapper;
        _roleManager = roleManager;
    }

    public override async Task<ICollection<UserResponse>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
    {
        var users = await _userManager.Users.AsNoTracking().ToListAsync(cancellationToken: cancellationToken);

        var userResponses = new List<UserResponse>();

        foreach (var user in users)
        {
            var userResponse = _mapper.Map<UserResponse>(user);
            
            var roleNames = await _userManager.GetRolesAsync(user);
            
            foreach (var roleName in roleNames)
            {
                var role = await _roleManager.FindByNameAsync(roleName);

                if (role is null)
                {
                    ArgumentNullException.ThrowIfNull(role);
                }

                var roleClaims = await _roleManager.GetClaimsAsync(role);

                userResponse.Roles.Add(new RoleResponse()
                {
                    Id = role.Id,
                    Name = role.Name,
                    NormalizedName = role.NormalizedName,
                    IsDefault = role.IsDefault,
                    Priority = role.Priority,
                    Permissions = roleClaims.Where(e => e.Type == AuthClaimTypes.Permission).Select(e => e.Value).ToList()
                });
                
                
            }
            userResponses.Add(userResponse);

        }

        return userResponses;
    }
}