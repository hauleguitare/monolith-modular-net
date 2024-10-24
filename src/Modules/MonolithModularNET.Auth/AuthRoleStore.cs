using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using MonolithModularNET.Auth.Core;

namespace MonolithModularNET.Auth;

public class AuthRoleStore: RoleStore<AuthRole>
{
    public AuthRoleStore(AuthContext context, IdentityErrorDescriber? describer = null) : base(context, describer)
    {
        AutoSaveChanges = false;
    }
}