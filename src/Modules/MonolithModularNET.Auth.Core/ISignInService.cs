using Microsoft.AspNetCore.Identity;

namespace MonolithModularNET.Auth.Core;

public interface ISignInService<TUser>: IDisposable where TUser : AuthUser
{
    IPasswordHasher<TUser> PasswordHasher { get; set; }
    
    public Task<SignInResult> SignInAsync(TUser user, string password, CancellationToken cancellationToken = default);

    public Task<SignInResult> RefreshAsync(string token, string userId, CancellationToken cancellationToken = default);
}