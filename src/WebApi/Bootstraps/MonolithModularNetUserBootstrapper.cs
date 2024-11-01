using Microsoft.EntityFrameworkCore;
using MonolithModularNET.User.Infrastructure;

namespace WebApi.Bootstraps;

public static class MonolithModularNetUserBootstrapper
{
    public static IServiceCollection AddUserBootstrapper(this IServiceCollection services, IConfiguration configuration,
        IWebHostEnvironment environment)
    {
        services.AddMonolithModularNetUser();

        services.AddMonolithModularNetUserDbContext(opts =>
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