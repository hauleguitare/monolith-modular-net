using MonolithModularNET.Extensions.Abstractions;

namespace MonolithModularNET.Extensions.Shared.Cqrs;

public interface ICqrsResult
{
    public IError[]? Errors { get; set; }
    public bool IsSuccess { get; set; }
}


public class CqrsResult: ICqrsResult
{
    public object? Result { get; set; }
    public IError[]? Errors { get; set; }
    public string[]? Messages { get; set; }
    public bool IsSuccess { get; set; } = false;
}

public class CqrsError: IError
{
    public string? Code { get; set; }
    public string? Description { get; set; }
}


public class CqrsResult<TResponse>: ICqrsResult
{
    public TResponse? Result { get; set; }
    public IError[]? Errors { get; set; }
    public bool IsSuccess { get; set; } = false;

    public static CqrsResult<TResponse> Failure(CqrsError[] errors)
    {
        return new CqrsResult<TResponse>()
        {
            IsSuccess = false,
            Errors = errors
        };
    }

    public static CqrsResult<TResponse> Succeed(TResponse data)
    {
        return new CqrsResult<TResponse>()
        {
            IsSuccess = true,
            Result = data
        };
    }
}