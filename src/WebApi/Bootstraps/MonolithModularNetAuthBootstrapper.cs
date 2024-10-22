using Microsoft.EntityFrameworkCore;
using MonolithModularNET.Auth;

namespace WebApi.Bootstraps;

internal static class MonolithModularNetAuthBootstrapper
{
    internal static IServiceCollection AddMonolithModularNetAuthBootstrapper(this IServiceCollection services,
        IConfiguration configuration, IWebHostEnvironment environment)
    {
        services.AddMonolithModularNetAuth(opts =>
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            opts.UseNpgsql(connectionString);

            if (environment.IsDevelopment())
            {
                opts.EnableSensitiveDataLogging();
                opts.EnableDetailedErrors();
            }
        });
        
        return services;
    }
}