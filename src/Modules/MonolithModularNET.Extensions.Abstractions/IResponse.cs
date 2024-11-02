namespace MonolithModularNET.Extensions.Abstractions;

public interface IResponse<TError> : IResponse<object, TError> where TError : IError
{
}


public interface IResponse<TResult, TError> where TError : IError
{
    bool IsSucceed { get; set; }
    TResult? Result { get; set; }
    public ICollection<TError>? Errors { get; set; }
}