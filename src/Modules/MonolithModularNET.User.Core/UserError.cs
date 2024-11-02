using MonolithModularNET.Extensions.Abstractions;

namespace MonolithModularNET.User.Core;

public class UserError: IError
{
    public string? Code { get; set; }
    public string? Description { get; set; }
}