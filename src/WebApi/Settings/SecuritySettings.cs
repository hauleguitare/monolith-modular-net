namespace WebApi.Settings;

public class SecuritySettings
{
    public string? JwtSecretKey { get; set; }
    public int CacheExpiresIn { get; set; }
}