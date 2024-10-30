namespace MonolithModularNET.Auth;

public record AuthV1ClassicTokenCreateRequest
{
    public string? Description { get; set; }
    
    public DateTime ExpiredAt { get; set; }
}