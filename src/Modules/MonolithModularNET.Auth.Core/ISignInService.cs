namespace MonolithModularNET.Auth.Core;

public interface ISignInService<TUser, TRole>: IDisposable where TUser : AuthUser where TRole : AuthRole
{
}