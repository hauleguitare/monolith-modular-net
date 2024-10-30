namespace MonolithModularNET.Auth.Core;

public record AuthV1ClassicTokenCreateRequest
{
    public string? Description { get; set; }
    
    public AuthV1ClassicTokenMetadata Metadata { get; set; } = new ();

    public ICollection<AuthV1ClassicTokenClaimRequest> Claims { get; set; } =
        new List<AuthV1ClassicTokenClaimRequest>();
}

public record AuthV1ClassicTokenClaimRequest
{
    public string ValueType { get; set; } = null!;
    
    public string Value { get; set; } = null!;
}