using Microsoft.AspNetCore.Http;

namespace MonolithModularNET.Extensions.Abstractions;

public interface IResponse<TError> : IResponse<object, TError> where TError : IErrorResponse
{
}


public interface IResponse<TResult, TError> where TError : IErrorResponse
{
    bool IsSucceed { get; set; }
    TResult? Result { get; set; }
    public ICollection<TError>? Errors { get; set; }
}