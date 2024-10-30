using Microsoft.AspNetCore.Http;
using MonolithModularNET.Auth.Core;

namespace MonolithModularNET.Auth;

public static class AuthApiHandler
{
    public static async Task<IResult> HandleSignUpAsync(SignUpRequest request, ISignUpService<AuthUser, AuthRole> service)
    {
        var result = await service.SignUpAsync(request);

        return !result.Succeed ? Results.BadRequest(AuthResponse.Failure(result.Errors ?? new List<AuthError>())) : Results.Ok(AuthResponse.Success());
    }

    public static async Task<IResult> HandleLoginAsync(SignInRequest request, HttpContext context,
        ISignInService<AuthUser> service)
    {
        var result = await service.SignInAsync(request.Email, request.Password);

        if (!result.Succeed)
        {
            return Results.BadRequest(AuthResponse.Failure(result.Errors!));
        }
        return Results.Ok(AuthResponse.Success(result.Data));
    }

    public static async Task<IResult> HandleRefreshAsync(RefreshTokenRequest request, HttpContext context,
        ISignInService<AuthUser> service)
    {
        var result = await service.RefreshAsync(request.RefreshToken);
            
        if (!result.Succeed)
        {
            return Results.BadRequest(AuthResponse.Failure(result.Errors!));
        }

        return Results.Ok(AuthResponse.Success(result.Data));
    }
    
    public static async Task<IResult> HandleLogoutAsync(HttpContext context,
        ISignInService<AuthUser> service)
    {
        var result = await service.LogoutAsync();
        
        if (!result.Succeed)
        {
            return Results.BadRequest(AuthResponse.Failure(result.Errors!));
        }
        
        return Results.Ok(AuthResponse.Success(result.Data));
    }
}