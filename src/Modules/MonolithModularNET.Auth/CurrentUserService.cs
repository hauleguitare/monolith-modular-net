using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using MonolithModularNET.Auth.Core;

namespace MonolithModularNET.Auth;

public class CurrentUserService: ICurrentUserService
{
    private readonly HttpContext _httpContext;

    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
    {
        ArgumentNullException.ThrowIfNull(httpContextAccessor.HttpContext);
        _httpContext = httpContextAccessor.HttpContext;
    }

    public ClaimsPrincipal ClaimsPrincipal => _httpContext.User;

    public string? UserId => ClaimsPrincipal.FindFirst(e => e.Type == ClaimTypes.NameIdentifier)?.Value;

    public bool IsAuthenticated => ClaimsPrincipal.Identity is { IsAuthenticated: true };

    public IEnumerable<ClaimsIdentity> ClaimsIdentities => ClaimsPrincipal.Identities;
}