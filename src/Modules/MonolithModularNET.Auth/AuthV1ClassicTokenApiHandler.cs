using Microsoft.AspNetCore.Http;
using MonolithModularNET.Auth.Core;
using MonolithModularNET.Extensions.Shared.Models;

namespace MonolithModularNET.Auth;

public class AuthV1ClassicTokenApiHandler
{
    public async Task<IResult> CreateAsync(AuthV1ClassicTokenCreateRequest request, IAuthV1ClassicTokenService service)
    {
        var result = await service.CreateAsync(request);
        
        if (!result.Succeed)
        {
            var errors = result.Errors == null
                ? new List<ApiErrorResponse>()
                : result.Errors.Select(e => new ApiErrorResponse()
                {
                    Code = e.Code,
                    Description = e.Description
                }).ToList();

            return Results.BadRequest(ApiResponse.Failure(errors.ToArray()));
        }

        return Results.Ok(ApiResponse.Success(result));
    }

    public async Task<IResult> GetAsync(IAuthV1ClassicTokenService service)
    {
        var result = await service.GetAsync();

        if (!result.Succeed)
        {
            var errors = result.Errors == null
                ? new List<ApiErrorResponse>()
                : result.Errors.Select(e => new ApiErrorResponse()
                {
                    Code = e.Code,
                    Description = e.Description
                }).ToList();

            return Results.BadRequest(ApiResponse.Failure(errors.ToArray()));
        }
        
        return Results.Ok(ApiResponse.Success(result));
    }

    public async Task<IResult> DeleteAsync(string token, IAuthV1ClassicTokenService service)
    {
        var result = await service.DeleteAsync(token);

        if (!result.Succeed)
        {
            var errors = result.Errors == null
                ? new List<ApiErrorResponse>()
                : result.Errors.Select(e => new ApiErrorResponse()
                {
                    Code = e.Code,
                    Description = e.Description
                }).ToList();

            return Results.BadRequest(ApiResponse.Failure(errors.ToArray()));
        }
        
        return Results.Ok(ApiResponse.Success(result));
    }
}