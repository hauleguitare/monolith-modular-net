using Microsoft.AspNetCore.Http;
using MonolithModularNET.Auth.Core;
using MonolithModularNET.Extensions.Shared.Models;

namespace MonolithModularNET.Auth;

public class AuthApiHandler
{
    public async Task<IResult> HandleSignUpAsync(SignUpRequest request, ISignUpService<AuthUser, AuthRole> service)
    {
        var result = await service.SignUpAsync(request);

        if (!result.Succeed)
        {
            var errors = result.Errors == null
                ? new List<ApiErrorResponse>()
                : result.Errors.Select(e => new ApiErrorResponse()
                {
                    Code = e.Code,
                    Description = e.Description
                }).ToList();

            return Results.BadRequest(ApiResponse.Failure(errors));
        }

        return Results.Ok(ApiResponse.Success(result.Data));
    }

    public async Task<IResult> HandleLoginAsync(SignInRequest request, HttpContext context,
        ISignInService<AuthUser> service)
    {
        var result = await service.SignInAsync(request.Email, request.Password);

        if (!result.Succeed)
        {
            var errors = result.Errors == null
                ? new List<ApiErrorResponse>()
                : result.Errors.Select(e => new ApiErrorResponse()
                {
                    Code = e.Code,
                    Description = e.Description
                }).ToList();

            return Results.BadRequest(ApiResponse.Failure(errors));
        }

        return Results.Ok(ApiResponse.Success(result.Data));
    }

    public async Task<IResult> HandleRefreshAsync(RefreshTokenRequest request, HttpContext context,
        ISignInService<AuthUser> service)
    {
        var result = await service.RefreshAsync(request.RefreshToken);
            
        if (!result.Succeed)
        {
            var errors = result.Errors == null
                ? new List<ApiErrorResponse>()
                : result.Errors.Select(e => new ApiErrorResponse()
                {
                    Code = e.Code,
                    Description = e.Description
                }).ToList();

            return Results.BadRequest(ApiResponse.Failure(errors));
        }

        return Results.Ok(ApiResponse.Success(result.Data));
    }
    
    public async Task<IResult> HandleLogoutAsync(HttpContext context,
        ISignInService<AuthUser> service)
    {
        var result = await service.LogoutAsync();
        
        if (!result.Succeed)
        {
            var errors = result.Errors == null
                ? new List<ApiErrorResponse>()
                : result.Errors.Select(e => new ApiErrorResponse()
                {
                    Code = e.Code,
                    Description = e.Description
                }).ToList();

            return Results.BadRequest(ApiResponse.Failure(errors));
        }

        return Results.Ok(ApiResponse.Success(result.Data));
    }
}