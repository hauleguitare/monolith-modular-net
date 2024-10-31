using Microsoft.AspNetCore.Http;
using MonolithModularNET.User.Core;

namespace MonolithModularNET.User;

public class UserApiHandler
{
    public async Task<IResult> GetByIdAsync(string id, IUserService userService)
    {
        var result = await userService.GetById(id);

        if (!result.Succeed)
        {
            return Results.BadRequest(result.Errors);
        }

        return Results.Ok(result.Data);
    }
}