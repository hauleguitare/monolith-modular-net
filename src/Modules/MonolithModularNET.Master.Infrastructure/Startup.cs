using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using MonolithModularNET.Master.Core;
using MonolithModularNET.Master.Core.Repositories;
using MonolithModularNET.Master.Infrastructure.Context;
using MonolithModularNET.Master.Infrastructure.Mapper;
using MonolithModularNET.Master.Infrastructure.Repositories;

namespace MonolithModularNET.Master.Infrastructure;

public static class Startup
{
    public static WebApplication MapMonolithModularNetMaster(this WebApplication app, [StringSyntax("Route")] string pattern = "/api")
    {
        var companyTaskApiHandler = new CompanyTaskApiHandler();
        var masterGroup = app.MapGroup($"{pattern}/master");
        var companyTaskGroup = masterGroup.MapGroup("tasks").RequireAuthorization();

        companyTaskGroup.MapGet("/", companyTaskApiHandler.GetAsync);
        companyTaskGroup.MapPost("/{taskId}/check-sessions", companyTaskApiHandler.CheckSessionAsync);

        var companyTaskSessionGroup = masterGroup.MapGroup("task-sessions");
        companyTaskSessionGroup.MapPost("/", companyTaskApiHandler.AddSessionAsync);
        
        return app;
    }
    
    public static IServiceCollection AddMonolithModularNetMasterDbContext(this IServiceCollection services,
        Action<DbContextOptionsBuilder>? optionsAction = null)
    {
        // Add AuthDbContext
        return services.AddDbContext<MasterDbContext>(optionsAction);
    }

    public static IServiceCollection AddMonolithModularNetMaster(this IServiceCollection services)
    {
        services.TryAddScoped(typeof(IMasterReadonlyRepository<>), typeof(MasterReadonlyRepository<>));
        services.TryAddScoped(typeof(IMasterWriteableRepository<>), typeof(MasterWritableRepository<>));

        // Add MediatR
        services.RegisterCqrs();
        
        // Add AutoMapper
        services.AddAutoMapper((e) =>
        {
            e.AddProfile<MasterRequestProfile>();
            e.AddProfile<MasterResponseProfile>();
        });
        
        // Add unit of work
        services.TryAddScoped<IMasterUnitOfWork, MasterUnitOfWork>();
        return services;
    }
    
    private static IServiceCollection RegisterCqrs(this IServiceCollection services)
    {
        var assembly = AppDomain.CurrentDomain.Load("MonolithModularNET.Master.Core");
        
        services.AddMediatR(conf =>
            conf.RegisterServicesFromAssembly(assembly));

        return services;
    }
}