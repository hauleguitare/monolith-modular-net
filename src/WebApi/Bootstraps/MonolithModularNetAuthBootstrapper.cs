using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MonolithModularNET.Auth;
using MonolithModularNET.Extensions.Shared.Authorization;
using WebApi.Settings;

namespace WebApi.Bootstraps;

internal static class MonolithModularNetAuthBootstrapper
{
    internal static IServiceCollection AddAuthBootstrapper(this IServiceCollection services,
        IConfiguration configuration, IWebHostEnvironment environment)
    {
        var securitySettings = new SecuritySettings();
        configuration.GetSection(nameof(SecuritySettings)).Bind(securitySettings);
        
        ArgumentNullException.ThrowIfNull(securitySettings.JwtSecretKey);

        var cacheSettings = new CacheSettings();
        configuration.GetSection(nameof(CacheSettings)).Bind(cacheSettings);
        
        ArgumentNullException.ThrowIfNull(cacheSettings.ConnectionString);
        ArgumentNullException.ThrowIfNull(cacheSettings.InstanceName);
        
        // add cache service
        services.AddMonolithModularNetAuthCache(options =>
        {
            options.ConnectionString = cacheSettings.ConnectionString;
            options.InstanceName = $"{cacheSettings.InstanceName}:auth:";
        });

        
        // Add auth module.
        services.AddMonolithModularNetAuthContext(opts =>
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            opts.UseNpgsql(connectionString);

            if (environment.IsDevelopment())
            {
                opts.EnableSensitiveDataLogging();
                opts.EnableDetailedErrors();
            }
        })
            .AddMonolithModularNetAuth()
            .AddAuthJwtTokenOptions(options =>
            {
                options.SecretKey = securitySettings.JwtSecretKey;
                options.ExpiresInMinutes = 1;
            });
        
        
        // Add authentication
        services.AddAuthentication(conf =>
        {
            conf.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;

        }).AddJwtBearer(options =>
        {
            options.RequireHttpsMetadata = !environment.IsDevelopment();
            options.TokenValidationParameters = new TokenValidationParameters()
            {
                ValidateIssuer = !environment.IsDevelopment(),
                ValidateAudience = !environment.IsDevelopment(),
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey =
                    new SymmetricSecurityKey(Encoding.UTF8.GetBytes(securitySettings.JwtSecretKey)),
            };
        });
        
        // Add authorization
        services.AddAuthorization(options =>
        {
            options.AddPolicy(nameof(RoleBasedRequirement), policy =>
            {
                policy.Requirements.Add(new RoleBasedRequirement());
            });
        });

        services.AddSingleton<IAuthorizationHandler, RoleBasedHandler>();
        
        return services;
    }
}