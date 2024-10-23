using MonolithModularNET.Extensions.Shared;

namespace WebApi.Bootstraps;

public static class CacheBootstrapper
{
    public static IServiceCollection AddCacheBootstrapper(this IServiceCollection services,
        IConfiguration configuration, IWebHostEnvironment environment)
    {
        services.AddMonolithModularNetCache(options =>
        {
            options.ConnectionString = "http://localhost:6379";
            options.InstanceName = "monolith-modular-net";
        });

        return services;
    }
}