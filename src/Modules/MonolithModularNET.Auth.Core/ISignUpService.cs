using Microsoft.AspNetCore.Identity;

namespace MonolithModularNET.Auth.Core;

public interface ISignUpService<TUser, TRole>: IDisposable where TUser : AuthUser where TRole : AuthRole
{
    public Task<IdentityResult> SignUpAsync(SignUpRequest request,
        CancellationToken cancellationToken = default(CancellationToken));
}