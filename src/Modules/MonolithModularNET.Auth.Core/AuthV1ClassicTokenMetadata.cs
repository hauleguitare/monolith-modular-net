using MonolithModularNET.Extensions.Abstractions;

namespace MonolithModularNET.Auth.Core;

public class AuthV1ClassicTokenMetadata: ValueObject
{
    public DateTime ExpiredAt { get; set; }

    public bool IsActive { get; set; }
    
    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return ExpiredAt;
        yield return IsActive;
    }
}