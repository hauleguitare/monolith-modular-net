namespace MonolithModularNET.Auth.Core;

public class AuthErrorDescriber
{
    public virtual AuthError DefaultError()
    {
        return new AuthError
        {
            Code = nameof(DefaultError),
            Description = "DefaultError"
        };
    }
    
    public virtual AuthError EmailNotExisted()
    {
        return new AuthError
        {
            Code = nameof(EmailNotExisted),
            Description = "EmailNotExisted"
        };
    }
    
    public virtual AuthError PasswordMisMatched()
    {
        return new AuthError
        {
            Code = nameof(PasswordMisMatched),
            Description = "PasswordMisMatched"
        };
    }
    
    public virtual AuthError RoleNotFound()
    {
        return new AuthError
        {
            Code = nameof(RoleNotFound),
            Description = "RoleNotFound"
        };
    }
    
    public virtual AuthError TokenHasExpired()
    {
        return new AuthError
        {
            Code = nameof(TokenHasExpired),
            Description = "TokenHasExpired"
        };
    }
    
    public virtual AuthError InvalidToken()
    {
        return new AuthError
        {
            Code = nameof(InvalidToken),
            Description = "InvalidToken"
        };
    }
    
    public virtual AuthError NotSupportPasswordProvider()
    {
        return new AuthError
        {
            Code = nameof(NotSupportPasswordProvider),
            Description = "NotSupportPasswordProvider"
        };
    }
    
}