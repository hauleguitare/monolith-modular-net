using Microsoft.AspNetCore.Builder;

namespace MonolithModularNET.Auth.Shared;

public static class RoleBaseAuthorizationEndpointConventionBuilderExtensions
{
   
    public static TBuilder RequirePermissions<TBuilder>(this TBuilder builder, params string[] permissions)
        where TBuilder : IEndpointConventionBuilder
    {
        builder.Add(endpointBuilder => { endpointBuilder.Metadata.Add(
            new RoleBasedAuthorizeAttribute(permissions)); });
        return builder;
    }
}