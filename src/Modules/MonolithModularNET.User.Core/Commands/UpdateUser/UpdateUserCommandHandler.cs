using MediatR;
using Microsoft.AspNetCore.Identity;
using MonolithModularNET.Auth.Core;
using MonolithModularNET.Extensions.Shared.Cqrs;
using MonolithModularNET.Extensions.Shared.Models;
using MonolithModularNET.User.Core.Queries.GetUserById;

namespace MonolithModularNET.User.Core.Commands.UpdateUser;

public class UpdateUserCommandHandler(UserManager<AuthUser> userManager, ISender sender): CqrsCommandHandler<UpdateUserCommand, UserResponse?>
{
    public override async Task<CqrsResult<UserResponse?>> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByEmailAsync(request.Email);
        var describer = new UserErrorDescriber();
        
        
        if (user is null)
        {
            var error = describer.EmailDoesNotExist();
            return CqrsResult<UserResponse?>.Failure(new []
            {
                new CqrsError()
                {
                    Code = error.Code,
                    Description = error.Description
                }
            });
        }

        user.FirstName = request.FirstName;
        user.LastName = request.LastName;
        user.IsActive = request.IsActive;
        user.AvatarUrl = request.AvatarUrl;
        user.PhoneNumber = request.PhoneNumber;


        var result = await userManager.UpdateAsync(user);

        if (!result.Succeeded)
        {
            return CqrsResult<UserResponse?>.Failure(result.Errors.Select(e => new CqrsError()
            {
                Description = e.Description,
                Code = e.Code
            }).ToArray());
        }

        var previousRoles = await userManager.GetRolesAsync(user);
        await userManager.RemoveFromRolesAsync(user, previousRoles);
        var addToRolesAsync = await userManager.AddToRolesAsync(user, request.RoleNames);

        if (!addToRolesAsync.Succeeded)
        {
            return CqrsResult<UserResponse?>.Failure(result.Errors.Select(e => new CqrsError()
            {
                Description = e.Description,
                Code = e.Code
            }).ToArray());
        }
        
        var userResponse = await sender.Send(new GetUserByIdQuery(user.Id), cancellationToken);

        return CqrsResult<UserResponse?>.Succeed(userResponse);
    }
}