using Microsoft.AspNetCore.Identity;

namespace MonolithModularNET.Auth.Core;

public class AuthRole: IdentityRole
{
    public override string Id { get; set; } = Guid.NewGuid().ToString();
    public int Priority { get; set; }
    public bool IsDefault { get; set; } = false;
}