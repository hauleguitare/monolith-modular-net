using MonolithModularNET.Extensions.Abstractions;

namespace MonolithModularNET.Auth.Core;

public class AuthError: IError
{
    public string? Code { get; set; }
    public string? Description { get; set; }
}