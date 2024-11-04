namespace MonolithModularNET.Auth.Core;

public interface IRefreshTokenService
{
    public string Encoding(GenerateRefreshTokenOptions tokenOptions);
    public TokenResult Validate(string jti, string secretKey, string token);
    public RefreshTokenValue Decoding(string token);
}