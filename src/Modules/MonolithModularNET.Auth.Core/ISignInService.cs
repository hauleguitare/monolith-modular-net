using Microsoft.AspNetCore.Identity;

namespace MonolithModularNET.Auth.Core;

public interface ISignInService<TUser>: IDisposable where TUser : AuthUser
{
    IPasswordHasher<TUser> PasswordHasher { get; set; }
    
    public Task<AuthResult> SignInAsync(string email, string password, CancellationToken cancellationToken = default);
    public Task<AuthResult> RefreshAsync(string refreshToken, CancellationToken cancellationToken = default);
}