namespace MonolithModularNET.Extensions.Abstractions;

public interface IErrorResponse
{
    public string? Code { get; set; }
    public string? Description { get; set; }
}