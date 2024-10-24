namespace MonolithModularNET.Auth.Core;

public class AuthResult
{
    public bool Succeed { get; set; }
    
    public object? Data { get; set; }
    
    public ICollection<AuthError>? Errors { get; set; }
    
    public static AuthResult Success(object? data = null) => new ()
    {
        Succeed = true,
        Data = data
    };

    public static AuthResult Failure(ICollection<AuthError> errors) => new ()
    {
        Succeed = false,
        Errors = errors
    };
}