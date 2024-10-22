using Microsoft.AspNetCore.Identity;

namespace MonolithModularNET.Auth.Core;

public class AuthUser: IdentityUser<string>
{
    public AuthUser()
    {
        Id = Guid.NewGuid().ToString("N");
    }
}