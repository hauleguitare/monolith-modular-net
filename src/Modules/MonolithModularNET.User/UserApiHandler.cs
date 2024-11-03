using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MonolithModularNET.Extensions.Shared.Models;
using MonolithModularNET.Extensions.Shared.Services;
using MonolithModularNET.User.Core.Commands.PatchUpdateUser;
using MonolithModularNET.User.Core.Commands.UpdateUser;
using MonolithModularNET.User.Core.Queries.GetUserById;
using MonolithModularNET.User.Core.Queries.GetUsers;

namespace MonolithModularNET.User;

public class UserApiHandler
{
    public async Task<IResult> GetAsync([FromServices] ISender sender)
    {
        var result = await sender.Send(new GetUsersQuery());

        return Results.Ok(ApiResponse.Success(result));
    }
    
    
    public async Task<IResult> GetByIdAsync(string userId, [FromServices] ISender sender)
    {
        var result = await sender.Send(new GetUserByIdQuery(userId));

        return Results.Ok(ApiResponse.Success(result));
    }

    public async Task<IResult> GetSelf([FromServices] ISender sender, [FromServices] ICurrentUserService currentUserService)
    {
        var userId = currentUserService.UserId;

        if (string.IsNullOrEmpty(userId))
        {
            return Results.Unauthorized();
        }

        var result = await sender.Send(new GetUserByIdQuery(userId));

        return Results.Ok(ApiResponse.Success(result));
    }

    public async Task<IResult> UpdateUser(string userId, [FromBody] UpdateUserCommand command, [FromServices] IMapper mapper, [FromServices] ISender sender)
    {
        var result = await sender.Send(command);

        if (!result.IsSuccess)
        {
            return Results.BadRequest(ApiResponse.Failure(result.Errors!));
        }

        return Results.Ok(ApiResponse.Success(result.Result));
    }

    public async Task<IResult> PatchUpdateUserSelf([FromBody] PatchUpdateUserCommand command, [FromServices] ICurrentUserService currentUserService,
        [FromServices] IMapper mapper, [FromServices] ISender sender)
    {
        if (!currentUserService.IsAuthenticated)
        {
            return Results.Unauthorized();
        }

        var result = await sender.Send(command);

        if (!result.IsSuccess)
        {
            return Results.BadRequest(ApiResponse.Failure(result.Errors!));
        }

        return Results.Ok(ApiResponse.Success(result.Result));
    }
}