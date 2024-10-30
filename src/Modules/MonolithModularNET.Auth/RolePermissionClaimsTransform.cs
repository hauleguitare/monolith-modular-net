using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using MonolithModularNET.Auth.Core;
using MonolithModularNET.Extensions.Shared.Cache;

namespace MonolithModularNET.Auth;

public class RolePermissionClaimsTransform(RoleManager<AuthRole> roleManager, IAuthCacheService cacheService) : IClaimsTransformation
{
    private const int ExpiresInHours = 24;

    public async Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
    {
        ClaimsIdentity claimsIdentity = new ClaimsIdentity();
        var currentRoleIds = principal.FindAll(ClaimTypes.Role).Select(e => e.Value).ToList();

        if (!principal.HasClaim(claim => claim.Type == AuthClaimTypes.Permission))
        {
            foreach (var currentRoleId in currentRoleIds)
            {
                var cacheQueryString = new CacheQueryBuilder().Append(AuthCacheSchemas.Roles).Append(currentRoleId)
                    .ToString();

                var cacheRole = await cacheService.GetAsync<CacheAuthRole>(cacheQueryString);

                if (cacheRole is null)
                {
                    var role = await roleManager.FindByIdAsync(currentRoleId);

                    if (role is null)
                    {
                        return principal;
                    }
                    
                    var roleClaims = await roleManager.GetClaimsAsync(role);

                    cacheRole = new CacheAuthRole()
                    {
                        Id = role.Id,
                        Priority = role.Priority,
                        ConcurrencyStamp = role.ConcurrencyStamp,
                        NormalizedName = role.NormalizedName,
                        IsDefault = role.IsDefault,
                        Claims = roleClaims.Where(e => e.Type == AuthClaimTypes.Permission).Select(e => new CacheAuthClaim()
                        {
                            Value = e.Value,
                            Type = e.Type,
                            ValueType = e.ValueType,
                            Properties = e.Properties,
                            Issuer = e.Issuer,
                            Subject = e.Subject,
                            OriginalIssuer = e.OriginalIssuer
                            
                        }).ToList()
                    };
                    
                    await AddCacheIfNullAsync(role, roleClaims);

                }
              
                foreach (var roleClaim in cacheRole.Claims)
                {
                    claimsIdentity.AddClaim(new Claim(AuthClaimTypes.Permission, roleClaim.Value!));
                }

            }
            
            principal.AddIdentity(claimsIdentity);
        }
        return principal;
    }

    private async Task AddCacheIfNullAsync(AuthRole role, ICollection<Claim> claims,
        CancellationToken cancellationToken = default)
    {
        var query = new CacheQueryBuilder().Append(AuthCacheSchemas.Roles).Append(role.Id);

        var dataCached = await cacheService.GetAsync<CacheAuthRole>(query.ToString(), cancellationToken);

        if (dataCached is null)
        {
            dataCached = new CacheAuthRole()
            {
                Id = role.Id,
                Priority = role.Priority,
                ConcurrencyStamp = role.ConcurrencyStamp,
                NormalizedName = role.NormalizedName,
                IsDefault = role.IsDefault,
                Claims = claims.Select(e => new CacheAuthClaim()
                {
                    Value = e.Value,
                    Type = e.Type,
                    ValueType = e.ValueType,
                    Properties = e.Properties,
                    Issuer = e.Issuer,
                    Subject = e.Subject,
                    OriginalIssuer = e.OriginalIssuer
                            
                }).ToList()
            };

            await cacheService.SetAsync(query.ToString(), dataCached, TimeSpan.FromHours(ExpiresInHours), cancellationToken);
        }
    }
}