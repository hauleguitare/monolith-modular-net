using MediatR;
using Microsoft.AspNetCore.Identity;
using MonolithModularNET.Auth.Core;
using MonolithModularNET.Extensions.Shared.Cqrs;
using MonolithModularNET.Extensions.Shared.Models;
using MonolithModularNET.User.Core.Queries.GetUserById;

namespace MonolithModularNET.User.Core.Commands.SetRoles;

public class SetRolesCommandHandler: CqrsCommandHandler<SetRolesCommand, UserResponse?>
{
    private UserManager<AuthUser> _userManager;
    private RoleManager<AuthRole> _roleManager;
    private ISender _sender;

    public SetRolesCommandHandler(UserManager<AuthUser> userManager, RoleManager<AuthRole> roleManager, ISender sender)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _sender = sender;
    }

    public override async Task<CqrsResult<UserResponse?>> Handle(SetRolesCommand request, CancellationToken cancellationToken)
    {
        var describer = new UserErrorDescriber();
        var user = await _userManager.FindByIdAsync(request.UserId);

        if (user is null)
        {
            return CqrsResult<UserResponse?>.Failure([describer.UserNotFound()]);
        }
        
        var result = await _userManager.AddToRolesAsync(user, request.RoleNames);

        if (!result.Succeeded)
        {
            return CqrsResult<UserResponse?>.Failure(result.Errors.Select(e => new CqrsError()
            {
                Description = e.Description,
                Code = e.Code
            }).ToArray());
        }

        var userResponse = await _sender.Send(new GetUserByIdQuery(user.Id));

        return CqrsResult<UserResponse?>.Succeed(userResponse);
    }
}