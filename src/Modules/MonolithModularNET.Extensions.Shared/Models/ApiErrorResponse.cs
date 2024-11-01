using MonolithModularNET.Extensions.Abstractions;

namespace MonolithModularNET.Extensions.Shared.Models;

public class ApiErrorResponse: IErrorResponse
{
    public virtual string? Code { get; set; }
    public virtual string? Description { get; set; }
}