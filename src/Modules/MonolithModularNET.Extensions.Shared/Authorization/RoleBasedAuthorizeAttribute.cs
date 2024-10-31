using Microsoft.AspNetCore.Authorization;

namespace MonolithModularNET.Extensions.Shared.Authorization;

public class RoleBasedAuthorizeAttribute(ICollection<string> resourceActions) : AuthorizeAttribute, IAuthorizationRequirementData
{
    private ICollection<string> ResourceActions { get; set; } = resourceActions;

    public IEnumerable<IAuthorizationRequirement> GetRequirements()
    {
        yield return new RoleBasedRequirement(ResourceActions);
    }
}