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
            options.SecretKey = "@00282jseaX#@!XDAS3213123!312345345XDk23dKngmf5Ygvewq@**64534312dascghgffsfsd@#dasd";
            options.ExpiresIn = 60;
        });
        return services;
    }
}