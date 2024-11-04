namespace MonolithModularNET.Auth.Core;

public class AuthResult : AuthResult<object>
{
    public new static AuthResult Success(object? data = null) => new ()
    {
        Succeed = true,
        Data = data
    };
    
    public new static AuthResult Failure(params AuthError[] errors) => new ()
    {
        Succeed = false,
        Errors = errors
    };
}


public class AuthResult<TResponse> where TResponse : class
{
    public bool Succeed { get; set; }
    
    public TResponse? Data { get; set; }
    
    public ICollection<AuthError>? Errors { get; set; }
    
    public static AuthResult<TResponse> Success(TResponse? data = null) => new ()
    {
        Succeed = true,
        Data = data
    };

    public static AuthResult<TResponse> Failure(params AuthError[] errors) => new ()
    {
        Succeed = false,
        Errors = errors
    };
}