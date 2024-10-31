using Microsoft.AspNetCore.Authorization;

namespace MonolithModularNET.Extensions.Shared.Authorization;

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