using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MonolithModularNET.Extensions.Shared.Models;
using MonolithModularNET.User.Core.Commands.UpdateRole;
using MonolithModularNET.User.Core.Queries.GetRoleById;
using MonolithModularNET.User.Core.Queries.GetRoles;

namespace MonolithModularNET.User;

public class RoleApiHandler
{
    public async Task<IResult> GetAsync([FromServices] ISender sender)
    {
        var result = await sender.Send(new GetRolesQuery());

        return Results.Ok(ApiResponse.Success(result));
    }

    public async Task<IResult> GetByIdAsync(string roleId, [FromServices] ISender sender)
    {
        var result = await sender.Send(new GetRoleByIdQuery(roleId));

        return Results.Ok(ApiResponse.Success(result));
    }

    public async Task<IResult> UpdateAsync(string roleId, [FromBody] UpdateRoleCommand command, [FromServices] ISender sender)
    {
        var result = await sender.Send(command);

        if (!result.IsSuccess)
        {
            return Results.BadRequest(ApiResponse.Failure(result.Errors!));
        }

        return Results.Ok(ApiResponse.Success(result));
    }
}