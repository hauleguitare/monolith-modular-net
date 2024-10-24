using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using MonolithModularNET.Auth.Core;
using MonolithModularNET.Extensions.Abstractions;

namespace MonolithModularNET.Auth;

public class SignInService: ISignInService<AuthUser>
{
    private readonly IJwtService _jwtService;
    private readonly IRefreshTokenService _refreshTokenService;
    private readonly ICacheService _cacheService;
    private readonly AuthJwtTokenOptions _options;
    private readonly UserManager<AuthUser> _userManager;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public SignInService( IJwtService jwtService, IPasswordHasher<AuthUser> passwordHasher, AuthJwtTokenOptions options, IRefreshTokenService refreshTokenService, ICacheService cacheService, UserManager<AuthUser> userManager, IHttpContextAccessor httpContextAccessor)
    {
        _jwtService = jwtService;
        PasswordHasher = passwordHasher;
        _options = options;
        _refreshTokenService = refreshTokenService;
        _cacheService = cacheService;
        _userManager = userManager;
        _httpContextAccessor = httpContextAccessor;
        
        ArgumentNullException.ThrowIfNull(_options.SecretKey);
    }

    public IPasswordHasher<AuthUser> PasswordHasher { get; set; }

    public async Task<AuthResult> SignInAsync(string email, string password, CancellationToken cancellationToken = default)
    {
        var describer = new AuthErrorDescriber();
        var user = await _userManager.FindByEmailAsync(email);

        if (user is null)
        {
            return AuthResult.Failure([describer.EmailNotExisted()]);
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

    private async Task<AuthResult> CredentialAsync(AuthUser user, CancellationToken cancellationToken = default)
    {
        var claims = new List<Claim>()
        {
            new (ClaimTypes.Email, user.Email!),
            new (ClaimTypes.NameIdentifier, user.Id),
        };

        var jti = Guid.NewGuid().ToString();
        var token = _jwtService.Encoding(jti, claims);
        var expiredAt = DateTime.UtcNow.AddDays(7);
        var expiresTime = expiredAt - DateTime.UtcNow;
        
        var rfTokenResult = _refreshTokenService.GenerateRefreshToken(jti, _options.SecretKey!, expiredAt);

        if (!rfTokenResult.Succeed)
        {
            throw new Exception("Refresh Token can't create, something went wrong!");
        }
        
        await CacheRefreshTokenAsync(user.Id, rfTokenResult.Token!, expiresTime, cancellationToken);
        
        return AuthResult.Success(new {AccessToken = token, RefreshToken = rfTokenResult.Token});
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
        
        var validTokenResult = _refreshTokenService.ValidateRefreshToken(decodeToken.Id, _options.SecretKey!, refreshToken);

        if (!validTokenResult.Succeed)
        {
            return AuthResult.Failure([describer.InvalidToken()]);
        }

        var cacheRefreshToken = await GetRefreshTokenAsync(userId);

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
            throw new Exception("User is not found, something went wrong, please check!");
        }

        return await CredentialAsync(user, cancellationToken);
    }

    private bool IsEqualRefreshToken(string client, string server)
    {
        return client == server;
    }

    private async Task CacheRefreshTokenAsync(string userId, string refreshToken, TimeSpan expiresTime, CancellationToken cancellationToken = default)
    {
        var key = $"{RefreshTokenConstants.CachePrefix}:{userId}";
        await _cacheService.SetAsync(key, refreshToken, expiresTime, cancellationToken);
    }
    
    private async Task<string?> GetRefreshTokenAsync(string userId)
    {
        var key = $"{RefreshTokenConstants.CachePrefix}:{userId}";
        return await _cacheService.GetAsync(key);
    }

    public void Dispose()
    {
        GC.SuppressFinalize(this);
    }
}