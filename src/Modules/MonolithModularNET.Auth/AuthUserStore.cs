using System.Globalization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MonolithModularNET.Auth.Core;

namespace MonolithModularNET.Auth;

public class AuthUserStore: UserStore<AuthUser>
{
    private readonly AuthDbContext _dbContext;
    
    public AuthUserStore(AuthDbContext dbContext, IdentityErrorDescriber describer = null) : base(dbContext, describer)
    {
        _dbContext = dbContext;
        AutoSaveChanges = false;
    }


    public override async Task AddToRoleAsync(AuthUser user, string roleName,
        CancellationToken cancellationToken = new CancellationToken())
    {
        ThrowIfDisposed();
        if (user == null)
        {
            throw new ArgumentNullException(nameof(user));
        }
        if (String.IsNullOrWhiteSpace(roleName))
        {
            throw new ArgumentException("ValueCannotBeNullOrEmpty", nameof(roleName));
        }

        var roleEntity = await _dbContext.Roles.SingleOrDefaultAsync(r => !string.IsNullOrEmpty(r.Name) && r.Name.ToUpper() == roleName.ToUpper(), cancellationToken: cancellationToken);
        if (roleEntity == null)
        {
            throw new InvalidOperationException(String.Format(CultureInfo.CurrentCulture,
                "RoleNotFound", nameof(roleName)));
        }

        var ur = new IdentityUserRole<string>() { UserId = user.Id, RoleId = roleEntity.Id };
        _dbContext.UserRoles.Add(ur);
    }
}