namespace MonolithModularNET.Auth.Core;

public class SignInResult
{
    public bool Succeed { get; set; }
    
    public string? AccessToken { get; set; }
    
    public string? RefreshToken { get; set; }
    
    public ICollection<AuthError>? Errors { get; set; }

    public static SignInResult Success(string accessToken, string refreshToken) => new SignInResult()
    {
        Succeed = true,
        AccessToken = accessToken,
        RefreshToken = refreshToken
    };

    public static SignInResult Failure(ICollection<AuthError> errors) => new SignInResult()
    {
        Succeed = false,
        Errors = errors
    };
}