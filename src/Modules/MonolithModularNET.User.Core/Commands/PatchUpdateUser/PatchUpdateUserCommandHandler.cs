using AutoMapper;
using Microsoft.AspNetCore.Identity;
using MonolithModularNET.Auth.Core;
using MonolithModularNET.Extensions.Shared.Cqrs;
using MonolithModularNET.Extensions.Shared.Models;

namespace MonolithModularNET.User.Core.Commands.PatchUpdateUser;

public class PatchUpdateUserCommandHandler(UserManager<AuthUser> userManager, IMapper mapper)
    : CqrsCommandHandler<PatchUpdateUserCommand, UserResponse?>
{
    

    public override async Task<CqrsResult<UserResponse?>> Handle(PatchUpdateUserCommand request, CancellationToken cancellationToken)
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

        if (result.Succeeded)
        {
            var userResp = mapper.Map<UserResponse>(user);
            return CqrsResult<UserResponse?>.Succeed(userResp);
        }
        

        return CqrsResult<UserResponse?>.Failure(result.Errors.Select(e => new CqrsError()
        {
            Description = e.Description,
            Code = e.Code
        }).ToArray());
    }
}