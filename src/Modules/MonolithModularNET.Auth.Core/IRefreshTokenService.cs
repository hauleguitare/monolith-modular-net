namespace MonolithModularNET.Auth.Core;

public interface IRefreshTokenService
{
    public TokenResult Encoding(GenerateRefreshTokenOptions tokenOptions);
    public TokenResult Decoding(string jti, string secretKey, string token);
}