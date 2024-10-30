using System.Diagnostics.CodeAnalysis;
using System.Text;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Caching.StackExchangeRedis;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using MonolithModularNET.Auth.Core;
using MonolithModularNET.Extensions.Abstractions;
using MonolithModularNET.Extensions.Shared.Cache;

namespace MonolithModularNET.Auth;

public static class Startup
{
    public static WebApplication MapMonolithModularNetAuthApi(this WebApplication app, [StringSyntax("Route")] string pattern = "/api/auth")
    {
        var authApiHandler = new AuthApiHandler();
        var group = app.MapGroup(pattern);
        group.MapPost("/sign-up", authApiHandler.HandleSignUpAsync);
        group.MapPost("/login", authApiHandler.HandleLoginAsync);
        group.MapPost("/refresh", authApiHandler.HandleRefreshAsync);
        group.MapPost("/logout", authApiHandler.HandleLogoutAsync);


        var authV1ClassicTokenGroup = group.MapGroup("classic-tokens");
        var authV1ClassicTokenApiHandler = new AuthV1ClassicTokenApiHandler();
        authV1ClassicTokenGroup.MapPost("/", authV1ClassicTokenApiHandler.CreateAsync).RequirePermissions("v1_classic_token:create");
        authV1ClassicTokenGroup.MapGet("/", authV1ClassicTokenApiHandler.GetAsync).RequirePermissions("v1_classic_token:read_all");
        return app;
    }

    public static IServiceCollection AddMonolithModularNetAuthCache(this IServiceCollection services,
        Action<CacheOptions>? cacheAction = null)
    {
        // Add distributed cache
        if (cacheAction is null)
        {
            services.AddSingleton<IAuthDistributedCache, AuthDistributedCache>(provider =>
            {
                var options = new RedisCacheOptions()
                {
                    Configuration = CacheOptionDefault.DefaultConnectionString,
                    InstanceName = CacheOptionDefault.DefaultInstanceName
                };

                return new AuthDistributedCache(options);
            });
        }
        else
        {
            var cacheOptions = new CacheOptions();
            cacheAction.Invoke(cacheOptions);
            services.AddSingleton<IAuthDistributedCache, AuthDistributedCache>(provider =>
            {
                var options = new RedisCacheOptions()
                {
                    Configuration = cacheOptions.ConnectionString,
                    InstanceName = cacheOptions.InstanceName
                };

                return new AuthDistributedCache(options);
            });
        }
        
        // Add service
        services.AddScoped<IAuthCacheService, AuthCacheService>();

        return services;
    }

    public static IServiceCollection AddMonolithModularNetAuthContext(this IServiceCollection services,
        Action<DbContextOptionsBuilder>? optionsAction = null)
    {
        // Add AuthDbContext
        return services.AddDbContext<AuthDbContext>(optionsAction);
    }
    
    public static IServiceCollection AddMonolithModularNetAuth(this IServiceCollection services)
    {
        // Add Identity Core
        services.AddIdentityCore<AuthUser>()
            .AddRoles<AuthRole>()
            .AddUserManager<AuthUserManager>()
            .AddUserStore<AuthUserStore>()
            .AddRoleStore<AuthRoleStore>()
            .AddEntityFrameworkStores<AuthDbContext>();
        
        // Add AuthRole
        services.TryAddScoped<IRoleValidator<AuthRole>, RoleValidator<AuthRole>>();
        services.TryAddScoped<RoleManager<AuthRole>>();
        services.TryAddScoped<IUserClaimsPrincipalFactory<AuthUser>, UserClaimsPrincipalFactory<AuthUser, AuthRole>>();
        
        // Add Auth Unit Of Work
        services.TryAddScoped<IUnitOfWork<AuthDbContext, IDbContextTransaction>, AuthUnitOfWork>();
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
        
        // Add Roles Claim transform
        services.AddTransient<IClaimsTransformation, RolePermissionClaimsTransform>();
        
        // Add Repository
        services.TryAddScoped(typeof(IAuthReadonlyRepository<>), typeof(AuthReadonlyRepository<>));
        services.TryAddScoped(typeof(IAuthWriteableRepository<>), typeof(AuthWriteableRepository<>));
        
        return services;
    }

    public static IServiceCollection AddAuthJwtTokenOptions(this IServiceCollection services,
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