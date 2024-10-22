using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MonolithModularNET.Auth.Core;

namespace MonolithModularNET.Auth;

public class AuthUserManager: UserManager<AuthUser>
{
    private readonly AuthContext _authContext;
    private readonly IRoleStore<AuthRole> _roleStore;
    
    
    public AuthUserManager(IUserStore<AuthUser> store, IOptions<IdentityOptions> optionsAccessor, IPasswordHasher<AuthUser> passwordHasher, IEnumerable<IUserValidator<AuthUser>> userValidators, IEnumerable<IPasswordValidator<AuthUser>> passwordValidators, ILookupNormalizer keyNormalizer, IdentityErrorDescriber errors, IServiceProvider services, ILogger<UserManager<AuthUser>> logger, AuthContext authContext, IRoleStore<AuthRole> roleStore) : base(store, optionsAccessor, passwordHasher, userValidators, passwordValidators, keyNormalizer, errors, services, logger)
    {
        _authContext = authContext;
        _roleStore = roleStore;
    }

    public override async Task<IdentityResult> AddToRoleAsync(AuthUser user, string roleName)
    {
        ThrowIfDisposed();
        ArgumentNullException.ThrowIfNull(user);

        var normalizedRole = NormalizeName(roleName);

        var role = await _roleStore.FindByNameAsync(normalizedRole, CancellationToken);

        var userRoles = await GetRolesAsync(user);
        
        if (userRoles.Contains(roleName))
        {
        }

        _authContext.UserRoles.Add(new IdentityUserRole<string>()
        {
            UserId = user.Id,
            RoleId = role!.Id!
        });
        
        return await UpdateUserAsync(user).ConfigureAwait(false);
    }

    public override async Task<IList<string>> GetRolesAsync(AuthUser user)
    {
        var roleIds = await _authContext.UserRoles.Where(e => e.UserId == user.Id)
            .Select(e => e.RoleId).ToListAsync();

        var roles = await _authContext.Roles.Where(e => roleIds.Contains(e.Id)).Select(e => e.Name!).ToListAsync();

        return roles;
    }
}