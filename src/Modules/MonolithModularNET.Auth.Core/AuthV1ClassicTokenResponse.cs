namespace MonolithModularNET.Auth.Core;

public class AuthV1ClassicTokenResponse
{
    public string Token { get; set; } = null!;
    public string? Description { get; set; }
    public AuthV1ClassicTokenMetadata Metadata { get; set; } = new();
    public virtual ICollection<AuthV1ClassicTokenClaimResponse> Claims { get; set; } = new HashSet<AuthV1ClassicTokenClaimResponse>();
    
    public DateTimeOffset? CreatedAt { get; set; }
    
    public DateTimeOffset? ModifiedAt { get; set; }
    
    public string? CreatedBy { get; set; }
    
    public string? ModifiedBy { get; set; }
}

public record AuthV1ClassicTokenClaimResponse
{
    public string ValueType { get; set; } = null!;
    
    public string Value { get; set; } = null!;
}