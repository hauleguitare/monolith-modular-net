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
    
    public virtual AuthError EmailDoesNotExist()
    {
        return new AuthError
        {
            Code = nameof(EmailDoesNotExist),
            Description = "EmailDoesNotExist"
        };
    }
    
    public virtual AuthError UserIsNotActive()
    {
        return new AuthError
        {
            Code = nameof(UserIsNotActive),
            Description = "UserIsNotActive"
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
    
    public virtual AuthError NotAuthenticated()
    {
        return new AuthError
        {
            Code = nameof(NotAuthenticated),
            Description = "NotAuthenticated"
        };
    }
}