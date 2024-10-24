using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace WebApi.Bootstraps;

public static class AuthenticationBootstrapper
{
    internal static IServiceCollection AddResourceAuthentication(this IServiceCollection services,
        IConfiguration configuration, IWebHostEnvironment environment)
    {
        ArgumentNullException.ThrowIfNull(configuration["Security:JwtSecretKey"]);
        
        services.AddAuthentication(conf =>
        {
            conf.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;

        }).AddJwtBearer(options =>
        {
            options.RequireHttpsMetadata = !environment.IsDevelopment();
            options.TokenValidationParameters = new TokenValidationParameters()
            {
                ValidateIssuer = !environment.IsDevelopment(),
                ValidateAudience = !environment.IsDevelopment(),
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey =
                    new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Security:JwtSecretKey"]!)),
            };
        });
        
        return services;
    }
}