using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using MonolithModularNET.Extensions.Abstractions;
using MonolithModularNET.Extensions.Shared.Cache;

namespace MonolithModularNET.Extensions.Shared;

public static class Startup
{
    private static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddScoped<ICacheService, CacheService>();

        return services;
    }
    
    public static IServiceCollection AddMonolithModularNetCache(this IServiceCollection services, Action<CacheOptions>? cacheAction = null)
    {
        if (cacheAction is null)
        {
            
            services.AddStackExchangeRedisCache(opts =>
            {
                opts.Configuration = CacheOptionDefault.DefaultConnectionString;
                opts.InstanceName = CacheOptionDefault.DefaultInstanceName;
            });
        }
        else
        {
            var cacheOptions = new CacheOptions();
            cacheAction.Invoke(cacheOptions);
            
            services.AddStackExchangeRedisCache(opts =>
            {
                opts.Configuration = cacheOptions.ConnectionString;
                opts.InstanceName = cacheOptions.InstanceName;
            });
        }

        services.AddServices();

        return services;
    }
}