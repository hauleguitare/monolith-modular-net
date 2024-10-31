using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using MonolithModularNET.Auth.Core;
using MonolithModularNET.Extensions.Abstractions;

namespace MonolithModularNET.Auth;

public class AuthV1ClassicTokenApiHandler
{
    public async Task<IResult> CreateAsync(AuthV1ClassicTokenCreateRequest request, IAuthV1ClassicTokenService service)
    {
        var response = await service.CreateAsync(request);
        
        if (!response.Succeed)
        {
            Results.BadRequest(response.Errors);
        }

        return Results.Ok(AuthResponse.Success(response.Data));
    }

    public async Task<IResult> GetAsync(IAuthV1ClassicTokenService service)
    {
        var response = await service.GetAsync();

        if (!response.Succeed)
        {
            Results.BadRequest(response.Errors);
        }

        return Results.Ok(AuthResponse.Success(response.Data));
    }

    public async Task<IResult> DeleteAsync(string token, IAuthV1ClassicTokenService service)
    {
        var response = await service.DeleteAsync(token);

        if (!response.Succeed)
        {
            Results.BadRequest(response.Errors);
        }

        return Results.Ok(AuthResponse.Success());
    }
}