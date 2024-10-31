using Microsoft.AspNetCore.Authorization;

namespace MonolithModularNET.Auth.Shared;

public class RoleBasedRequirement : IAuthorizationRequirement
{
    public RoleBasedRequirement()
    {
    }
    
    public RoleBasedRequirement(ICollection<string> resourceActions)
    {
        ResourceActions = resourceActions;
    }

    public ICollection<string> ResourceActions { get; set; } = new List<string>();
}