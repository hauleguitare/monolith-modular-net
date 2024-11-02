
using MonolithModularNET.Extensions.Abstractions;

namespace MonolithModularNET.Extensions.Shared.Models;

public class ApiResponse: IResponse<ApiErrorResponse>
{
    public object? Result { get; set; }
    public bool IsSucceed { get; set; }
    public ICollection<ApiErrorResponse>? Errors { get; set; }

    public static ApiResponse Success(object? data = null)
    {
        return new ApiResponse()
        {
            IsSucceed = true,
            Result = data
        };
    }
    

    public static ApiResponse Failure(ICollection<IError> errors)
    {
        return new ApiResponse()
        {
            IsSucceed = false,
            Errors = errors.Select(e => new ApiErrorResponse()
            {
                Code = e.Code,
                Description = e.Description
            }).ToList()
        };
    }
    
}