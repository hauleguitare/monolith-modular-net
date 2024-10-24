namespace MonolithModularNET.Auth.Core;

public class TokenResult
{
    public bool Succeed { get; set; }
    public string? Token { get; set; }
    
    public ICollection<AuthError> Errors = new List<AuthError>();

    public static TokenResult Success() => new TokenResult()
    {
        Succeed = true
    };
    
    public static TokenResult Success(string token) => new TokenResult()
    {
        Succeed = true,
        Token = token
    };

    public static TokenResult Failure(ICollection<AuthError> errors) => new TokenResult()
    {
        Succeed = false,
        Errors = errors
    };
}