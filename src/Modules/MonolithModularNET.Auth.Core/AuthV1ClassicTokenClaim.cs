using MonolithModularNET.Extensions.Abstractions;

namespace MonolithModularNET.Auth.Core;

public class AuthV1ClassicTokenClaim: BaseEntity<int>
{
    public string Token { get; set; } = null!;
    
    public string ValueType { get; set; } = null!;
    
    public string Value { get; set; } = null!;
}