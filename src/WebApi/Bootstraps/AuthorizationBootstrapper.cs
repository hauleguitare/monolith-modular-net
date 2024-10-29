using Microsoft.AspNetCore.Authorization;
using MonolithModularNET.Auth;
using MonolithModularNET.Auth.Core;

namespace WebApi.Bootstraps;

public static class AuthorizationBootstrapper
{
    internal static IServiceCollection AddResourceAuthorization(this IServiceCollection services,
        IConfiguration configuration, IWebHostEnvironment environment)
    {
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