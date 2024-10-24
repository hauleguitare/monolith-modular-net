namespace WebApi.Bootstraps;

public static class AuthorizationBootstrapper
{
    internal static IServiceCollection AddResourceAuthorization(this IServiceCollection services,
        IConfiguration configuration, IWebHostEnvironment environment)
    {
        services.AddAuthorization();
        return services;
    }
}