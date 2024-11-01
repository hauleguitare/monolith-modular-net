using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using MonolithModularNET.User.Core;
using MonolithModularNET.User.Infrastructure.Context;
using MonolithModularNET.User.Infrastructure.Repositories;

namespace MonolithModularNET.User.Infrastructure;

public static class Startup
{
    public static IServiceCollection AddMonolithModularNetUser(this IServiceCollection services)
    {
        services.TryAddScoped(typeof(IUserReadonlyRepository<>), typeof(UserReadonlyRepository<>));
        services.TryAddScoped(typeof(IUserWriteableRepository<>), typeof(UserWriteableRepository<>));

        return services;
    }
    
    public static IServiceCollection AddMonolithModularNetUserDbContext(this IServiceCollection services,
        Action<DbContextOptionsBuilder>? optionsAction = null)
    {
        // Add AuthDbContext
        return services.AddDbContext<UserDbContext>(optionsAction);
    }
}