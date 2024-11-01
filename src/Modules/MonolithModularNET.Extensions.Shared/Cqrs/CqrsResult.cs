namespace MonolithModularNET.Extensions.Shared.Cqrs;

public interface ICqrsResult
{
    public CqrsError[]? Errors { get; set; }
    public bool IsSuccess { get; set; }
}


public class CqrsResult: ICqrsResult
{
    public object? Result { get; set; }
    public CqrsError[]? Errors { get; set; }
    public string[]? Messages { get; set; }
    public bool IsSuccess { get; set; } = false;
}

public class CqrsError
{
    public string? Code { get; set; }
    public string? Description { get; set; }
}


public class CqrsResult<TResponse>: ICqrsResult
{
    public TResponse? Result { get; set; }
    public CqrsError[]? Errors { get; set; }
    public string[]? Messages { get; set; }
    public bool IsSuccess { get; set; } = false;
}