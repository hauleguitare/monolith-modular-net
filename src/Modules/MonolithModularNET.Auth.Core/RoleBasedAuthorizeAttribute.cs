using Microsoft.AspNetCore.Authorization;

namespace MonolithModularNET.Auth.Core;

public class RoleBasedAuthorizeAttribute(string resourceAction) : AuthorizeAttribute, IAuthorizationRequirementData
{
    private string ResourceAction { get; set; } = resourceAction;

    public IEnumerable<IAuthorizationRequirement> GetRequirements()
    {
        yield return new RoleBasedRequirement(ResourceAction);
    }
}