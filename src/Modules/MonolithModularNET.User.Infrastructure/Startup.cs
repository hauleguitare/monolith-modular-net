using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using MonolithModularNET.User.Infrastructure.Mapper;

namespace MonolithModularNET.User.Infrastructure;

public static class Startup
{
    public static WebApplication MapMonolithModularNetUserApi(this WebApplication app,
        [StringSyntax("Route")] string pattern = "/api/users")
    {
        var userApiHandler = new UserApiHandler();
        var group = app.MapGroup(pattern);
        group.MapGet("/self", userApiHandler.GetSelf).RequireAuthorization();
        group.MapGet("/{userId}", userApiHandler.GetByIdAsync).RequireAuthorization();
        group.MapPatch("/{userId}", userApiHandler.PatchUpdateUser).RequireAuthorization();

        return app;
    }
    
    
    public static IServiceCollection AddMonolithModularNetUser(this IServiceCollection services)
    {
        // Add MediatR
        services.RegisterCqrs();
        
        // Add AutoMapper
        services.AddAutoMapper((e) =>
        {
            e.AddProfile<UserRequestProfile>();
            e.AddProfile<UserResponseProfile>();
        });
        
        return services;
    }
    

    private static IServiceCollection RegisterCqrs(this IServiceCollection services)
    {
        var assembly = AppDomain.CurrentDomain.Load("MonolithModularNET.User");
        
        services.AddMediatR(conf =>
            conf.RegisterServicesFromAssembly(assembly));

        return services;
    }
}