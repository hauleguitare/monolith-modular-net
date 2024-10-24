using MonolithModularNET.Extensions.Abstractions;

namespace MonolithModularNET.Auth;

public class AuthErrorResponse: IErrorResponse
{
    public virtual string? Code { get; set; }
    public virtual string? Description { get; set; }
}