namespace MonolithModularNET.Auth.Core;

public class GenerateRefreshTokenOptions
{
    public string? Jti { get; set; }
    public string? SecretKey { get; set; }
    public DateTime? ExpiredAt { get; set; }
}