using Microsoft.AspNetCore.Authorization;
using MonolithModularNET.Auth.Core;

namespace MonolithModularNET.Auth;

public class RoleBasedHandler: AuthorizationHandler<RoleBasedRequirement>
{

    public RoleBasedHandler()
    {
    }

    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, RoleBasedRequirement requirement)
    {
        if (context.User.Identity is null || !context.User.Identity.IsAuthenticated)
        {
            context.Fail();
            return Task.CompletedTask;
        }

        if (string.IsNullOrEmpty(requirement.ResourceAction))
        {
            return Task.CompletedTask;
        }
        

        var hasAccessibility = HasAccessibility(context, requirement.ResourceAction);

        if (hasAccessibility)
        {
            context.Succeed(requirement);
        }
        else
        {
            context.Fail();
        }
        
        return Task.CompletedTask;
    }


    private bool HasAccessibility(AuthorizationHandlerContext context, string resourceAction, CancellationToken cancellationToken = default)
    {
        var permissions = context.User.FindAll(claims => claims.Type == AuthClaimTypes.Permission).Select(e => e.Value).ToList();
        
        if (!permissions.Any())
        {
            return false;
        }

        return permissions.Any(e => e == resourceAction);
    }
}