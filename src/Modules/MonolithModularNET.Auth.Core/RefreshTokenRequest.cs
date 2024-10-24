namespace MonolithModularNET.Auth.Core;

public record RefreshTokenRequest(string RefreshToken, string UserId)
{
}