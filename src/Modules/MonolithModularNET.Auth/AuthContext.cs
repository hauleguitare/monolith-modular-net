using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MonolithModularNET.Auth.Core;

namespace MonolithModularNET.Auth;

public class AuthContext: IdentityDbContext<AuthUser, AuthRole, string>
{
    public AuthContext(DbContextOptions<AuthContext> options) : base(options)
    {
        
    }
}