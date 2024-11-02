namespace MonolithModularNET.Extensions.Abstractions;

public interface IError
{
    public string? Code { get; set; }
    public string? Description { get; set; }
}