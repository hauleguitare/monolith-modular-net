using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using MonolithModularNET.Extensions.Shared.Authorization;
using MonolithModularNET.Extensions.Shared.Permissions;
using MonolithModularNET.User.Infrastructure.Mapper;

namespace MonolithModularNET.User.Infrastructure;

public static class Startup
{
    public static WebApplication MapMonolithModularNetUserApi(this WebApplication app,
        [StringSyntax("Route")] string pattern = "/api/users")
    {
        var userApiHandler = new UserApiHandler();
        var group = app.MapGroup(pattern).RequireAuthorization();
        group.MapGet("/", userApiHandler.GetAsync).RequirePermissions(UserPermissions.ViewAll);
        group.MapGet("/{userId}", userApiHandler.GetByIdAsync);
        group.MapPut("/{userId}", userApiHandler.UpdateUser).RequirePermissions(UserPermissions.Update, UserPermissions.SetRoles);
        group.MapGet("/self", userApiHandler.GetSelf);
        group.MapPatch("/self", userApiHandler.PatchUpdateUserSelf);
        return app;
    }

    public static WebApplication MapMonolithModularNetRoleApi(this WebApplication app,
        [StringSyntax("Route")] string pattern = "/api/roles")
    {
        var roleApiHandler = new RoleApiHandler();
        var group = app.MapGroup(pattern).RequireAuthorization();

        group.MapGet("/", roleApiHandler.GetAsync).RequirePermissions(RolePermissions.ViewAll);
        group.MapGet("/{roleId}", roleApiHandler.GetByIdAsync).RequirePermissions(RolePermissions.ViewAll);
        group.MapPut("/{roleId}", roleApiHandler.UpdateAsync).RequirePermissions(RolePermissions.Update, RolePermissions.SetPermissions);

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
        var assembly = AppDomain.CurrentDomain.Load("MonolithModularNET.User.Core");
        
        services.AddMediatR(conf =>
            conf.RegisterServicesFromAssembly(assembly));

        return services;
    }
}