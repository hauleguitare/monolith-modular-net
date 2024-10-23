using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using MonolithModularNET.Auth.Core;
using SignInResult = MonolithModularNET.Auth.Core.SignInResult;

namespace MonolithModularNET.Auth;

public class SignInService: ISignInService<AuthUser>
{
    private readonly IJwtService _jwtService;

    public SignInService( IJwtService jwtService, IPasswordHasher<AuthUser> passwordHasher)
    {
        _jwtService = jwtService;
        PasswordHasher = passwordHasher;
    }

    public IPasswordHasher<AuthUser> PasswordHasher { get; set; }

    public Task<SignInResult> SignInAsync(AuthUser user, string password, CancellationToken cancellationToken = default)
    {
        var verificationResult = PasswordHasher.VerifyHashedPassword(user, user.PasswordHash, password);

        if (verificationResult == PasswordVerificationResult.Failed)
        {
            throw new Exception("Password is not matched");
        }
        
        var claims = new List<Claim>()
        {
            new Claim(ClaimTypes.Email, user.Email!),
            new Claim(ClaimTypes.NameIdentifier, user.Id),
        };

        var token = _jwtService.Encoding(claims);

        return Task.FromResult(SignInResult.Success(token));
    }

    public void Dispose()
    {
        GC.SuppressFinalize(this);
    }
}