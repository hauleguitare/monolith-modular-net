using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MonolithModularNET.Extensions.Shared.Models;
using MonolithModularNET.Extensions.Shared.Services;
using MonolithModularNET.User.Commands.PatchUpdateUser;
using MonolithModularNET.User.Queries.GetUserById;

namespace MonolithModularNET.User;

public class UserApiHandler
{
    public async Task<IResult> GetByIdAsync(string userId, [FromServices] ISender sender)
    {
        var result = await sender.Send(new GetUserByIdQuery(userId));

        return Results.Ok(result);
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

    public async Task<IResult> PatchUpdateUser(string userId, [FromBody] PatchUpdateUserCommand command, [FromServices] ISender sender)
    {
        var result = await sender.Send(command);

        if (!result.IsSuccess)
        {
            return Results.BadRequest(ApiResponse.Failure(result.Errors!));
        }

        return Results.Ok(ApiResponse.Success(result.Result));
    }
}