using System.Diagnostics.CodeAnalysis;
using System.Text;
using Microsoft.AspNetCore.Builder;
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
    public static WebApplication MapMonolithModularNetAuthApi(this WebApplication app, [StringSyntax("Route")] string pattern = "/api/auth")
    {
        var group = app.MapGroup(pattern);
        group.MapPost("/sign-up", AuthApiHandler.HandleSignUpAsync);
        group.MapPost("/login", AuthApiHandler.HandleLoginAsync);
        group.MapPost("/refresh", AuthApiHandler.HandleRefreshAsync);
        return app;
    }
    
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
        services.TryAddScoped<IRoleValidator<AuthRole>, RoleValidator<AuthRole>>();
        services.TryAddScoped<RoleManager<AuthRole>>();
        services.TryAddScoped<IUserClaimsPrincipalFactory<AuthUser>, UserClaimsPrincipalFactory<AuthUser, AuthRole>>();
        
        // Add Auth Unit Of Work
        services.TryAddScoped<IUnitOfWork<AuthContext, IDbContextTransaction>, AuthUnitOfWork>();
        // Add AuthService
        services.TryAddScoped<ISignUpService<AuthUser, AuthRole>, SignUpService>();
        // Add JWTService
        services.TryAddScoped<IJwtService, JwtService>();
        // Add RefreshTokenService
        services.TryAddScoped<IRefreshTokenService, RefreshTokenService>();
        // Add SignInService
        services.TryAddScoped<ISignInService<AuthUser>, SignInService>();
        // Add Http Context Accessor
        services.AddHttpContextAccessor();
        
        return services;
    }

    public static IServiceCollection AddAuthJwtToken(this IServiceCollection services,
        Action<AuthJwtTokenOptions>? options = null)
    {
        var jwtTokenOptions = new AuthJwtTokenOptions();

        if (options is null)
        {
            services.TryAddTransient(typeof(AuthJwtTokenOptions), provider => new AuthJwtTokenOptions()
            {
                SecretKey = "bOBL7HWpP898C3zkWKQS8Uqa5ZWX/7UnSM5yRWOSZWTennHj5ZESA917+8Nlx65L",
                ExpiresIn = 120
            });
        }
        else
        {
            options.Invoke(jwtTokenOptions);
            
            ArgumentNullException.ThrowIfNull(jwtTokenOptions.SecretKey);

            if (Encoding.UTF8.GetBytes(jwtTokenOptions.SecretKey).Length != 64)
            {
                throw new ArgumentException($"{nameof(jwtTokenOptions.SecretKey)} must be 64 bytes length");
            }
            
            services.TryAddTransient(typeof(AuthJwtTokenOptions), provider => new AuthJwtTokenOptions()
            {
                SecretKey = jwtTokenOptions.SecretKey,
                ExpiresIn = jwtTokenOptions.ExpiresIn
            });
        }
        return services;
    }
}