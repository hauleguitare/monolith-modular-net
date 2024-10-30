using Microsoft.AspNetCore.Http;
using MonolithModularNET.Auth.Core;

namespace MonolithModularNET.Auth;

public class AuthApiHandler
{
    public async Task<IResult> HandleSignUpAsync(SignUpRequest request, ISignUpService<AuthUser, AuthRole> service)
    {
        var result = await service.SignUpAsync(request);

        return !result.Succeed ? Results.BadRequest(AuthResponse.Failure(result.Errors ?? new List<AuthError>())) : Results.Ok(AuthResponse.Success());
    }

    public async Task<IResult> HandleLoginAsync(SignInRequest request, HttpContext context,
        ISignInService<AuthUser> service)
    {
        var result = await service.SignInAsync(request.Email, request.Password);

        return !result.Succeed ? Results.BadRequest(AuthResponse.Failure(result.Errors!)) : Results.Ok(AuthResponse.Success(result.Data));
    }

    public async Task<IResult> HandleRefreshAsync(RefreshTokenRequest request, HttpContext context,
        ISignInService<AuthUser> service)
    {
        var result = await service.RefreshAsync(request.RefreshToken);
            
        return !result.Succeed ? Results.BadRequest(AuthResponse.Failure(result.Errors!)) : Results.Ok(AuthResponse.Success(result.Data));
    }
    
    public async Task<IResult> HandleLogoutAsync(HttpContext context,
        ISignInService<AuthUser> service)
    {
        var result = await service.LogoutAsync();
        
        return !result.Succeed ? Results.BadRequest(AuthResponse.Failure(result.Errors!)) : Results.Ok(AuthResponse.Success(result.Data));
    }
}