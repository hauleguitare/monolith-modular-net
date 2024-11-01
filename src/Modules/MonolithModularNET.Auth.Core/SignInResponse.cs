using MonolithModularNET.Extensions.Shared.Models;

namespace MonolithModularNET.Auth.Core;

public class SignInResponse
{
    public string? AccessToken { get; set; }
    public string? RefreshToken { get; set; }
    public UserResponse? User { get; set; }
}