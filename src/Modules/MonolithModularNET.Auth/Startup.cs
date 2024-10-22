using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using MonolithModularNET.Auth.Core;
using MonolithModularNET.Extensions.Abstractions;

namespace MonolithModularNET.Auth;

public static class Startup
{
    public static IServiceCollection AddMonolithModularNetAuth(this IServiceCollection services,  Action<DbContextOptionsBuilder>? optionsAction = null)
    {
        // Add AuthContext
        services.AddDbContext<AuthContext>(optionsAction);
        
        
        // Add Identity Core
        services.AddIdentityCore<AuthUser>()
            .AddRoles<AuthRole>()
            .AddUserManager<AuthUserManager>()
            .AddUserStore<AuthUserStore>()
            .AddRoleStore<AuthRoleStore>()
            .AddEntityFrameworkStores<AuthContext>();
        
        // Add AuthRole
        services.AddAuthRole();
        
        // Add Auth Unit Of Work
        services.TryAddScoped<IUnitOfWork<AuthContext, IDbContextTransaction>, AuthUnitOfWork>();
        
        // Add AuthService
        services.TryAddScoped<ISignUpService<AuthUser, AuthRole>, SignUpService>();
        
        // Add JWTService
        services.TryAddScoped<IJwtService, JwtService>();
        
        return services;
    }


    private static IServiceCollection AddAuthRole(this IServiceCollection services)
    {
        services.TryAddScoped<IRoleValidator<AuthRole>, RoleValidator<AuthRole>>();
        services.TryAddScoped<RoleManager<AuthRole>>();
        services.TryAddScoped<IUserClaimsPrincipalFactory<AuthUser>, UserClaimsPrincipalFactory<AuthUser, AuthRole>>();
        
        return services;
    }
}