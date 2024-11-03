using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using MonolithModularNET.Auth.Core;

namespace MonolithModularNET.Auth;

public class AuthRoleManager: RoleManager<AuthRole>
{
    public AuthRoleManager(IRoleStore<AuthRole> store, IEnumerable<IRoleValidator<AuthRole>> roleValidators, ILookupNormalizer keyNormalizer, IdentityErrorDescriber errors, ILogger<RoleManager<AuthRole>> logger) : base(store, roleValidators, keyNormalizer, errors, logger)
    {
    }
}