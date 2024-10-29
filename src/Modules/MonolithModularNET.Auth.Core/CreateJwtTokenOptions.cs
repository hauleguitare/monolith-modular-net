using System.Security.Claims;

namespace MonolithModularNET.Auth.Core;

public class CreateJwtTokenOptions
{
    public string? Id { get; set; }
    public string? SecretKey { get; set; }
    public DateTime? ExpiredAt { get; set; }
    
    public string? Issuer { get; set; }

    public ICollection<Claim> Claims = new List<Claim>();
}