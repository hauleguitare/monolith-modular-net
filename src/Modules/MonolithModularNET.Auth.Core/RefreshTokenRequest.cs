namespace MonolithModularNET.Auth.Core;

public record RefreshTokenRequest(string RequestToken, string UserId)
{
}