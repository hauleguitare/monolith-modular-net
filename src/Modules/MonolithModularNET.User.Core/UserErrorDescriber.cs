using MonolithModularNET.Auth.Core;

namespace MonolithModularNET.User.Core;

public class UserErrorDescriber
{
    public virtual UserError DefaultError()
    {
        return new UserError
        {
            Code = nameof(DefaultError),
            Description = "DefaultError"
        };
    }
    
    public virtual UserError EmailDoesNotExist()
    {
        return new UserError
        {
            Code = nameof(EmailDoesNotExist),
            Description = "EmailDoesNotExist"
        };
    }
    
    
    public virtual UserError RoleNotFound()
    {
        return new UserError
        {
            Code = nameof(RoleNotFound),
            Description = "RoleNotFound"
        };
    }
}