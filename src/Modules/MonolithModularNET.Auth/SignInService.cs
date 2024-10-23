using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using MonolithModularNET.Auth.Core;
using MonolithModularNET.Extensions.Abstractions;
using SignInResult = MonolithModularNET.Auth.Core.SignInResult;

namespace MonolithModularNET.Auth;

public class SignInService: ISignInService<AuthUser>
{
    private readonly IJwtService _jwtService;
    private readonly IRefreshTokenService _refreshTokenService;
    private readonly ICacheService _cacheService;
    private readonly AuthJwtTokenOptions _options;

    public SignInService( IJwtService jwtService, IPasswordHasher<AuthUser> passwordHasher, AuthJwtTokenOptions options, IRefreshTokenService refreshTokenService, ICacheService cacheService)
    {
        _jwtService = jwtService;
        PasswordHasher = passwordHasher;
        _options = options;
        _refreshTokenService = refreshTokenService;
        _cacheService = cacheService;
    }

    public IPasswordHasher<AuthUser> PasswordHasher { get; set; }

    public async Task<SignInResult> SignInAsync(AuthUser user, string password, CancellationToken cancellationToken = default)
    {
        var verificationResult = PasswordHasher.VerifyHashedPassword(user, user.PasswordHash, password);

        if (verificationResult == PasswordVerificationResult.Failed)
        {
            throw new Exception("Password is not matched");
        }
        
        var claims = new List<Claim>()
        {
            new Claim(ClaimTypes.Email, user.Email!),
            new Claim(ClaimTypes.NameIdentifier, user.Id),
        };

        var jti = Guid.NewGuid().ToString();
        var token = _jwtService.Encoding(jti, claims);
        var expiredAt = DateTime.UtcNow.AddDays(7);
        var expiresTime = expiredAt - DateTime.UtcNow;
        
        var rfTokenResult = _refreshTokenService.GenerateRefreshToken(jti, _options.SecretKey!, expiredAt);

        if (!rfTokenResult.Succeed)
        {
            throw new Exception("Refresh Token can't create");
        }
        _refreshTokenService.ValidateRefreshToken(jti, _options.SecretKey!, rfTokenResult.Token!);

        await CacheRefreshTokenAsync(user.Id, rfTokenResult.Token!, expiresTime, cancellationToken);

        return SignInResult.Success(token, rfTokenResult.Token!);
    }

    public async Task<SignInResult> RefreshAsync(string token, string userId, CancellationToken cancellationToken = default)
    {
        var cacheToken = await GetRefreshTokenAsync(userId);

        if (string.IsNullOrEmpty(cacheToken))
        {
            throw new Exception("Please login in again");
        }

        if (cacheToken != token)
        {
            throw new Exception("Token is not match");
        }

        return SignInResult.Success("", "");
    }

    private async Task CacheRefreshTokenAsync(string userId, string refreshToken, TimeSpan expiresTime, CancellationToken cancellationToken = default)
    {
        var key = $"refresh-token-user-id:{userId}";
        await _cacheService.SetAsync(key, refreshToken, expiresTime, cancellationToken);
    }
    
    private async Task<string?> GetRefreshTokenAsync(string userId)
    {
        var key = $"refresh-token-user-id:{userId}";
        return await _cacheService.GetAsync(key);
    }

    public void Dispose()
    {
        GC.SuppressFinalize(this);
    }
}