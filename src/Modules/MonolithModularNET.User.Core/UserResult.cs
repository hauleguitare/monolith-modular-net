namespace MonolithModularNET.User.Core;


public class UserResult : UserResult<object>
{
    public new static UserResult Success(object? data = null) => new ()
    {
        Succeed = true,
        Data = data
    };
    
    public new static UserResult Failure(ICollection<UserError> errors) => new ()
    {
        Succeed = false,
        Errors = errors
    };
}


public class UserResult<TResponse> where TResponse : new()
{
    public bool Succeed { get; set; }
    
    public TResponse? Data { get; set; }
    
    public ICollection<UserError>? Errors { get; set; }
    
    public static UserResult<TResponse> Success(TResponse? data) => new ()
    {
        Succeed = true,
        Data = data
    };

    public static UserResult<TResponse> Failure(ICollection<UserError> errors) => new ()
    {
        Succeed = false,
        Errors = errors
    };
}