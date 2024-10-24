using System.Globalization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MonolithModularNET.Auth.Core;

namespace MonolithModularNET.Auth;

public class AuthUserStore: UserStore<AuthUser>
{
    private readonly AuthContext _context;
    
    public AuthUserStore(AuthContext context, IdentityErrorDescriber describer = null) : base(context, describer)
    {
        _context = context;
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

        var roleEntity = await _context.Roles.SingleOrDefaultAsync(r => !string.IsNullOrEmpty(r.Name) && r.Name.ToUpper() == roleName.ToUpper(), cancellationToken: cancellationToken);
        if (roleEntity == null)
        {
            throw new InvalidOperationException(String.Format(CultureInfo.CurrentCulture,
                "RoleNotFound", nameof(roleName)));
        }

        var ur = new IdentityUserRole<string>() { UserId = user.Id, RoleId = roleEntity.Id };
        _context.UserRoles.Add(ur);
    }
}