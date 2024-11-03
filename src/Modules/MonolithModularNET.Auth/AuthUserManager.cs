using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MonolithModularNET.Auth.Core;

namespace MonolithModularNET.Auth;

public class AuthUserManager: UserManager<AuthUser>
{
    private readonly AuthDbContext _authDbContext;
    private readonly IRoleStore<AuthRole> _roleStore;
    
    
    public AuthUserManager(IUserStore<AuthUser> store, IOptions<IdentityOptions> optionsAccessor, IPasswordHasher<AuthUser> passwordHasher, IEnumerable<IUserValidator<AuthUser>> userValidators, IEnumerable<IPasswordValidator<AuthUser>> passwordValidators, ILookupNormalizer keyNormalizer, IdentityErrorDescriber errors, IServiceProvider services, ILogger<UserManager<AuthUser>> logger, AuthDbContext authDbContext, IRoleStore<AuthRole> roleStore) : base(store, optionsAccessor, passwordHasher, userValidators, passwordValidators, keyNormalizer, errors, services, logger)
    {
        _authDbContext = authDbContext;
        _roleStore = roleStore;
    }

    public override async Task<IdentityResult> AddToRolesAsync(AuthUser user, IEnumerable<string> roles)
    {
        ThrowIfDisposed();
        ArgumentNullException.ThrowIfNull(user);

        foreach (var roleName in roles)
        {
            var normalizedRole = NormalizeName(roleName);

            var role = await _roleStore.FindByNameAsync(normalizedRole, CancellationToken);

            _authDbContext.UserRoles.Add(new IdentityUserRole<string>()
            {
                UserId = user.Id,
                RoleId = role!.Id!
            });
        }
        
        return await UpdateUserAsync(user).ConfigureAwait(false);
    }

    public override async Task<IdentityResult> RemoveFromRolesAsync(AuthUser user, IEnumerable<string> roles)
    {
        ThrowIfDisposed();
        ArgumentNullException.ThrowIfNull(user);

        var describer = new IdentityErrorDescriber();
        foreach (var roleName in roles)
        {
            var normalizedRole = NormalizeName(roleName);

            var role = await _roleStore.FindByNameAsync(normalizedRole, CancellationToken);

            if (role is null)
            {
                return IdentityResult.Failed(describer.InvalidRoleName(roleName));
            }

            var userRole = await _authDbContext.UserRoles.Where(e => e.UserId == user.Id && e.RoleId == role.Id).FirstOrDefaultAsync();

            if (userRole is not null)
            {
                _authDbContext.UserRoles.Remove(userRole);
            }
        }
        
        return await UpdateUserAsync(user).ConfigureAwait(false);
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

        _authDbContext.UserRoles.Add(new IdentityUserRole<string>()
        {
            UserId = user.Id,
            RoleId = role!.Id!
        });
        
        return await UpdateUserAsync(user).ConfigureAwait(false);
    }

    public override async Task<IList<string>> GetRolesAsync(AuthUser user)
    {
        var roleIds = await _authDbContext.UserRoles.Where(e => e.UserId == user.Id)
            .Select(e => e.RoleId).ToListAsync();

        var roles = await _authDbContext.Roles.Where(e => roleIds.Contains(e.Id)).Select(e => e.Name!).ToListAsync();

        return roles;
    }
}