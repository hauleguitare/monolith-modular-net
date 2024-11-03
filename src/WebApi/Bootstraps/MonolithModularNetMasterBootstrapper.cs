using Microsoft.EntityFrameworkCore;
using MonolithModularNET.Master.Infrastructure;

namespace WebApi.Bootstraps;

public static class MonolithModularNetMasterBootstrapper
{
    public static IServiceCollection AddMasterBootstrapper(this IServiceCollection services, IConfiguration configuration,
        IWebHostEnvironment environment)
    {
        services.AddMonolithModularNetMasterDbContext(opts =>
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            ArgumentNullException.ThrowIfNull(connectionString);
            
            opts.UseMySQL(connectionString);

            if (environment.IsDevelopment())
            {
                opts.EnableSensitiveDataLogging();
                opts.EnableDetailedErrors();
            }
        }).AddMonolithModularNetMaster();

        return services;
    }
}