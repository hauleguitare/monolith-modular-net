using MonolithModularNET.Extensions.Abstractions;

namespace MonolithModularNET.Auth.Core;

public class AuthV1ClassicToken: IAuditableEntity, IAggregateRoot
{
    public string Token { get; set; } = null!;
    public string? Description { get; set; }
    public AuthV1ClassicTokenMetadata Metadata { get; set; } = new ();
    public virtual ICollection<AuthV1ClassicTokenClaim> Claims { get; set; } = new HashSet<AuthV1ClassicTokenClaim>();
    
    public DateTimeOffset? CreatedAt { get; set; }
    
    public DateTimeOffset? ModifiedAt { get; set; }
    
    public string? CreatedBy { get; set; }
    
    public string? ModifiedBy { get; set; }
}