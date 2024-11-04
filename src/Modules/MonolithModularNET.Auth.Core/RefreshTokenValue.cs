namespace MonolithModularNET.Auth.Core;

public class RefreshTokenValue
{
    public string? Jti { get; set; }
    public DateTime? ExpiredAt { get; set; }
}