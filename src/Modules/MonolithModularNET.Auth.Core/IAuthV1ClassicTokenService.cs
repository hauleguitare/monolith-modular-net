namespace MonolithModularNET.Auth.Core;

public interface IAuthV1ClassicTokenService
{
    public Task<AuthResult<AuthV1ClassicTokenResponse>> CreateAsync(AuthV1ClassicTokenCreateRequest request);

    public Task<AuthResult<ICollection<AuthV1ClassicTokenResponse>>> GetAsync();
    Task<AuthResult> DeleteAsync(string tokenId);
}