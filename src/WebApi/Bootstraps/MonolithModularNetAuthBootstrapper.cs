using Microsoft.EntityFrameworkCore;
using MonolithModularNET.Auth;

namespace WebApi.Bootstraps;

internal static class MonolithModularNetAuthBootstrapper
{
    internal static IServiceCollection AddAuthBootstrapper(this IServiceCollection services,
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

        services.AddAuthJwtToken(options =>
        {
            options.SecretKey = "bOBL7HWpP898C3zkWKQS8Uqa5ZWX/7UnSM5yRWOSZWTennHj5ZESA917+8Nlx65L";
            options.ExpiresIn = 60;
        });
        return services;
    }
}