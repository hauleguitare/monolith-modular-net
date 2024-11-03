using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using MonolithModularNET.Auth.Core;

namespace MonolithModularNET.Auth;

public class AuthRoleStore: RoleStore<AuthRole>
{
    public AuthRoleStore(AuthDbContext dbContext, IdentityErrorDescriber? describer = null) : base(dbContext, describer)
    {
    }
}