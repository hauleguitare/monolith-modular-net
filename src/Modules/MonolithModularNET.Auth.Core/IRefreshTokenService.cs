namespace MonolithModularNET.Auth.Core;

public interface IRefreshTokenService
{
    public TokenResult GenerateRefreshToken(string jti, string secretKey, DateTime expiredAt);
    public TokenResult ValidateRefreshToken(string jti, string secretKey, string token);
}