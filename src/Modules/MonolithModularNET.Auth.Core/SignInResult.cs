namespace MonolithModularNET.Auth.Core;

public class SignInResult
{
    public bool Succeed { get; set; }
    
    public string? AccessToken { get; set; }
    
    public ICollection<AuthError>? Errors { get; set; }

    public static SignInResult Success(string accessToken) => new SignInResult()
    {
        Succeed = true,
        AccessToken = accessToken
    };

    public static SignInResult Failure(ICollection<AuthError> errors) => new SignInResult()
    {
        Succeed = false,
        Errors = errors
    };
}