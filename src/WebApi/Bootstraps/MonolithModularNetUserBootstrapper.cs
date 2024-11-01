using Microsoft.EntityFrameworkCore;
using MonolithModularNET.User.Infrastructure;

namespace WebApi.Bootstraps;

public static class MonolithModularNetUserBootstrapper
{
    public static IServiceCollection AddUserBootstrapper(this IServiceCollection services, IConfiguration configuration,
        IWebHostEnvironment environment)
    {
        services.AddMonolithModularNetUser();

        return services;
    }
}