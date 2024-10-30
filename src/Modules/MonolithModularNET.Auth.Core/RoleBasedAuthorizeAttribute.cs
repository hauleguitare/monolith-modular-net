using Microsoft.AspNetCore.Authorization;

namespace MonolithModularNET.Auth.Core;

public class RoleBasedAuthorizeAttribute(ICollection<string> resourceActions) : AuthorizeAttribute, IAuthorizationRequirementData
{
    private ICollection<string> ResourceActions { get; set; } = resourceActions;

    public IEnumerable<IAuthorizationRequirement> GetRequirements()
    {
        yield return new RoleBasedRequirement(ResourceActions);
    }
}