namespace MonolithModularNET.Auth.Core;

public static class RefreshTokenDescriber
{
    public static AuthError RefreshTokenExpiredTime()
    {
        return new AuthError()
        {
            Code = "RefreshTokenExpiredTime",
            Description = "RefreshTokenExpiredTime"
        };
    }

    public static AuthError RefreshTokenJtiNotMatch()
    {
        return new AuthError()
        {
            Code = "RefreshTokenJtiNotMatch",
            Description = "RefreshTokenJtiNotMatch"
        };
    }
    
    public static AuthError RefreshTokenSecretKeyNotMatch()
    {
        return new AuthError()
        {
            Code = "RefreshTokenSecretKeyNotMatch",
            Description = "RefreshTokenSecretKeyNotMatch"
        };
    }
}
