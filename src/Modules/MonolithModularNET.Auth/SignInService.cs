using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using MonolithModularNET.Auth.Core;
using MonolithModularNET.Extensions.Shared.Cache;
using MonolithModularNET.Extensions.Shared.Models;

namespace MonolithModularNET.Auth;

public class SignInService: ISignInService<AuthUser>
{
    private readonly IJwtService _jwtService;
    private readonly IRefreshTokenService _refreshTokenService;
    private readonly AuthJwtTokenOptions _options;
    private readonly UserManager<AuthUser> _userManager;
    private readonly RoleManager<AuthRole> _roleManager;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IAuthCacheService _cacheService;

    public SignInService( IJwtService jwtService, IPasswordHasher<AuthUser> passwordHasher, AuthJwtTokenOptions options, IRefreshTokenService refreshTokenService, UserManager<AuthUser> userManager, IHttpContextAccessor httpContextAccessor, IAuthCacheService cacheService, RoleManager<AuthRole> roleManager)
    {
        _jwtService = jwtService;
        PasswordHasher = passwordHasher;
        _options = options;
        _refreshTokenService = refreshTokenService;
        _userManager = userManager;
        _httpContextAccessor = httpContextAccessor;
        _cacheService = cacheService;
        _roleManager = roleManager;

        ArgumentNullException.ThrowIfNull(_options.SecretKey);
    }

    public IPasswordHasher<AuthUser> PasswordHasher { get; set; }

    public async Task<AuthResult> SignInAsync(string email, string password, CancellationToken cancellationToken = default)
    {
        var describer = new AuthErrorDescriber();
        var user = await _userManager.FindByEmailAsync(email);

        if (user is null)
        {
            return AuthResult.Failure([describer.EmailDoesNotExist()]);
        }

        if (!user.IsActive)
        {
            return AuthResult.Failure([describer.UserIsNotActive()]);
        }

        if (string.IsNullOrEmpty(user.PasswordHash))
        {
            return AuthResult.Failure([describer.NotSupportPasswordProvider()]);
        }
        
        var verificationResult = PasswordHasher.VerifyHashedPassword(user, user.PasswordHash, password);

        if (verificationResult == PasswordVerificationResult.Failed)
        {
            return AuthResult.Failure([describer.PasswordMisMatched()]);
        }

        return await CredentialAsync(user, cancellationToken);
    }

    public async Task<AuthResult> LogoutAsync(CancellationToken cancellationToken = default)
    {
        var describer = new AuthErrorDescriber();

        if (!TryGetAccessToken(out var accessToken))
        {
            return AuthResult.Failure([describer.InvalidToken()]);
        }
        
        var decodeToken = _jwtService.Decoding(accessToken!);
        var userId = decodeToken.Claims.FirstOrDefault(e => e.Type == ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(userId))
        {
            return AuthResult.Failure([describer.InvalidToken()]);
        }

        var refreshTokenQuery = new CacheQueryBuilder().Append(AuthCacheSchemas.RefreshTokenUser).Append(userId).ToString();
        await _cacheService.RemoveAsync(refreshTokenQuery, cancellationToken);
        
        return AuthResult.Success();
    }

    private async Task<ICollection<Claim>> GrantClaimsAsync(AuthUser user)
    {
        // Refresh token operation
        var claims = new List<Claim>()
        {
            new (ClaimTypes.Email, user.Email!),
            new (ClaimTypes.NameIdentifier, user.Id),
        };
        
        // Models roles operation

        var roleNames = await _userManager.GetRolesAsync(user);
        
        foreach (var roleName in roleNames)
        {
            var role = await _roleManager.FindByNameAsync(roleName);
            
            
            claims.Add(new Claim(ClaimTypes.Role, role!.Id));
        }
        return claims;
    }
    
    private async Task<AuthResult> CredentialAsync(AuthUser user, CancellationToken cancellationToken = default)
    {
        var claims = await GrantClaimsAsync(user);
        
        // Generate token
        var jti = Guid.NewGuid().ToString();
        var token = _jwtService.Encoding(new CreateJwtTokenOptions()
        {
            Id = jti,
            Claims = claims,
            ExpiredAt = DateTime.UtcNow.AddMinutes(_options.ExpiresInMinutes),
            SecretKey = _options.SecretKey,
            Issuer = _options.Issuer,
            Audience = _options.Audience
        });
        var expiredAt = DateTime.UtcNow.AddDays(7);
        var expiresTime = expiredAt - DateTime.UtcNow;
        
        var rfTokenResult = _refreshTokenService.Encoding(new GenerateRefreshTokenOptions()
        {
            Jti = jti,
            SecretKey =  _options.SecretKey!,
             ExpiredAt =  expiredAt
        });

        if (!rfTokenResult.Succeed)
        {
            throw new Exception("Refresh Token can't create, something went wrong!");
        }
        await SetRefreshTokenCacheAsync(user.Id, rfTokenResult.Token!, expiresTime, cancellationToken);


        var userResponse = new UserResponse()
        {
            Id = user.Id,
            UserName = user.UserName!,
            AvatarUrl = user.AvatarUrl,
            PhoneNumber = user.PhoneNumber,
            PhoneNumberConfirmed = user.PhoneNumberConfirmed,
            EmailConfirmed = user.EmailConfirmed,
            Email = user.Email!,
            FirstName = user.FirstName,
            LastName = user.LastName,
            IsActive = user.IsActive
        };
        
        var userRoleNames = await _userManager.GetRolesAsync(user);

        foreach (var userRoleName in userRoleNames)
        {
            var role = await _roleManager.FindByNameAsync(userRoleName);

            if (role is null)
            {
                ArgumentNullException.ThrowIfNull(role);
            }

            var roleClaims = await _roleManager.GetClaimsAsync(role);

            userResponse.Roles.Add(new RoleResponse()
            {
                Id = role.Id,
                Name = role.Name,
                NormalizedName = role.NormalizedName,
                IsDefault = role.IsDefault,
                Priority = role.Priority,
                Permissions = roleClaims.Where(e => e.Type == AuthClaimTypes.Permission).Select(e => e.Value).ToList()
            });
        }

        return AuthResult.Success(new SignInResponse()
        {
            AccessToken = token,
            RefreshToken = rfTokenResult.Token,
            User = userResponse 
        });
    }

    private bool TryGetAccessToken(out string? accessToken)
    {
        var token = _httpContextAccessor.HttpContext!.Request.Headers.Authorization.ToString();
        accessToken = null;
        
        if (string.IsNullOrEmpty(token))
        {
            return false;
        }
        
        accessToken = token.Replace("Bearer ", "");

        return true;
    }

    public async Task<AuthResult> RefreshAsync(string refreshToken, CancellationToken cancellationToken = default)
    {
        var describer = new AuthErrorDescriber();

        if (!TryGetAccessToken(out var accessToken))
        {
            return AuthResult.Failure([describer.InvalidToken()]);
        }
        
        var decodeToken = _jwtService.Decoding(accessToken!);
        var userId = decodeToken.Claims.FirstOrDefault(e => e.Type == ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(userId))
        {
            return AuthResult.Failure([describer.InvalidToken()]);
        }
        
        var validTokenResult = _refreshTokenService.Decoding(decodeToken.Id, _options.SecretKey!, refreshToken);

        if (!validTokenResult.Succeed)
        {
            return AuthResult.Failure([describer.InvalidToken()]);
        }

        var cacheRefreshToken = await FindRefreshTokenAsync(userId);

        if (string.IsNullOrEmpty(cacheRefreshToken))
        {
            return AuthResult.Failure([describer.TokenHasExpired()]);
        }
        
        if (!IsEqualRefreshToken(refreshToken, cacheRefreshToken))
        {
            return AuthResult.Failure([describer.InvalidToken()]);
        }

        var user = await _userManager.FindByIdAsync(userId);

        if (user is null)
        {
            throw new Exception("Models is not found, something went wrong, please check!");
        }

        return await CredentialAsync(user, cancellationToken);
    }

    private bool IsEqualRefreshToken(string client, string server)
    {
        return client == server;
    }

    private async Task SetRefreshTokenCacheAsync(string userId, string refreshToken, TimeSpan expiresTime, CancellationToken cancellationToken = default)
    {
        var cacheQueryBuilder = new CacheQueryBuilder();
        var key = cacheQueryBuilder.Append(AuthCacheSchemas.RefreshTokenUser)
            .Append(userId).ToString();
        await _cacheService.SetAsync(key, refreshToken, expiresTime, cancellationToken);
    }
    
    
    private async Task<string?> FindRefreshTokenAsync(string userId)
    {
        var key = $"{AuthCacheSchemas.RefreshTokenUser}:{userId}";
        return await _cacheService.GetAsync(key);
    }

    public void Dispose()
    {
        GC.SuppressFinalize(this);
    }
}