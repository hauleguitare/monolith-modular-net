using Microsoft.AspNetCore.Identity;

namespace MonolithModularNET.Auth.Core;

public class AuthRole: IdentityRole
{
    public int Priority { get; set; }
    public bool IsDefault { get; set; } = false;
}