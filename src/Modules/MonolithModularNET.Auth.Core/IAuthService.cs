namespace MonolithModularNET.Auth.Core;

public interface IAuthService<TUser, TRole> : ISignUpService<TUser, TRole>,
    ISignInService<TUser, TRole> where TUser : AuthUser where TRole : AuthRole
{
    
}