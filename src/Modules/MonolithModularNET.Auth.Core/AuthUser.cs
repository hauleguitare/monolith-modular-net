using Microsoft.AspNetCore.Identity;

namespace MonolithModularNET.Auth.Core;

public class AuthUser: IdentityUser<string>
{
    public AuthUser()
    {
        Id = Guid.NewGuid().ToString("N");
    }
    
    public string? FirstName { get; set; }
    
    public string? LastName { get; set; }
}