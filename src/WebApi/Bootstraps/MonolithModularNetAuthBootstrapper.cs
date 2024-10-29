using Microsoft.EntityFrameworkCore;
using MonolithModularNET.Auth;

namespace WebApi.Bootstraps;

internal static class MonolithModularNetAuthBootstrapper
{
    internal static IServiceCollection AddAuthBootstrapper(this IServiceCollection services,
        IConfiguration configuration, IWebHostEnvironment environment)
    {
        ArgumentNullException.ThrowIfNull(configuration["Security:JwtSecretKey"]);
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
            .AddAuthJwtToken(options =>
            {
                options.SecretKey = configuration["Security:JwtSecretKey"];
                options.ExpiresIn = 60;
            });
        return services;
    }
}