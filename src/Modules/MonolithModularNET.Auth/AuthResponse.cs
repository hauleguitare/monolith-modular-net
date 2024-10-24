using Microsoft.AspNetCore.Http;
using MonolithModularNET.Auth.Core;
using MonolithModularNET.Extensions.Abstractions;

namespace MonolithModularNET.Auth;

public class AuthResponse: IResponse<AuthErrorResponse>
{
    public object? Result { get; set; }
    public bool IsSucceed { get; set; }
    public ICollection<AuthErrorResponse>? Errors { get; set; }

    public static AuthResponse Success(object? data = null)
    {
        return new AuthResponse()
        {
            IsSucceed = true,
            Result = data
        };
    }
    
    public static AuthResponse Failure(ICollection<AuthErrorResponse> errors)
    {
        return new AuthResponse()
        {
            IsSucceed = false,
            Errors = errors
        };
    }
    
    public static AuthResponse Failure(ICollection<AuthError> errors)
    {
        return new AuthResponse()
        {
            IsSucceed = false,
            Errors = errors.Select(e => new AuthErrorResponse()
            {
                Code = e.Code,
                Description = e.Description
            }).ToList()
        };
    }
    
}