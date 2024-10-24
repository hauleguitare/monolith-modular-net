namespace MonolithModularNET.Auth;

public class AuthJwtTokenOptions
{
    public string? Issuer { get; set; }
    public string? SecretKey { get; set; }
    public int ExpiresIn { get; set; }
}