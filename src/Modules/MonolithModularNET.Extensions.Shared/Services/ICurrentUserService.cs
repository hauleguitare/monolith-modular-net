using System.Security.Claims;

namespace MonolithModularNET.Extensions.Shared.Services;

public interface ICurrentUserService
{
    string? UserId { get; }
    public bool IsAuthenticated { get; }
    ClaimsPrincipal ClaimsPrincipal { get; }
    public IEnumerable<ClaimsIdentity> ClaimsIdentities { get; }
}