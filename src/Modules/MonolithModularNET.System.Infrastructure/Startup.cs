using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MonolithModularNET.System.Core;

namespace MonolithModularNET.System.Infrastructure;

public static class Startup
{
    public static IServiceCollection AddMonolithModularNetSystem(this IServiceCollection services)
    {
        services.AddScoped(typeof(ISystemReadonlyRepository<>), typeof(SystemReadonlyRepository<>));
        services.AddScoped(typeof(ISystemWriteableRepository<>), typeof(SystemWriteableRepository<>));
        
        return services;
    }

    public static IServiceCollection AddMonolithModularNetSystemDbContext(this IServiceCollection services, Action<DbContextOptionsBuilder>? optionsAction = null)
    {
        return services.AddDbContext<SystemDbContext>(optionsAction);
    }
}