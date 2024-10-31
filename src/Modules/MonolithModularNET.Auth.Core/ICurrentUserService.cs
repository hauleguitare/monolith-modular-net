using System.Security.Claims;

namespace MonolithModularNET.Auth.Core;

public interface ICurrentUserService
{
    string? UserId { get; }
    public bool IsAuthenticated { get; }
    ClaimsPrincipal ClaimsPrincipal { get; }
    public IEnumerable<ClaimsIdentity> ClaimsIdentities { get; }
}