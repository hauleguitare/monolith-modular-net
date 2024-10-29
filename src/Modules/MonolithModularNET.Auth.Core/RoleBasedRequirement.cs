using Microsoft.AspNetCore.Authorization;

namespace MonolithModularNET.Auth.Core;

public class RoleBasedRequirement : IAuthorizationRequirement
{
    public RoleBasedRequirement()
    {
    }
    
    public RoleBasedRequirement(string resourceAction)
    {
        ResourceAction = resourceAction;
    }
    
    public string? ResourceAction { get; set; }
}