namespace MonolithModularNET.Auth.Core;

public class AuthV1ClassicToken
{
    public string Token { get; set; } = null!;
    
    public string? Description { get; set; }

    public AuthV1ClassicTokenMetadata Metadata { get; set; } = new ();

    public virtual ICollection<AuthV1ClassicTokenClaim> Claims { get; set; } = new HashSet<AuthV1ClassicTokenClaim>();
}