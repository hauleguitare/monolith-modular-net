namespace WebApi.Settings;

public class SecuritySettings
{
    public string? JwtSecretKey { get; set; }
    public int JwtExpiresInMinutes { get; set; } = 60;
    public string? Issuer { get; set; }
    public string? Audience { get; set; }

}