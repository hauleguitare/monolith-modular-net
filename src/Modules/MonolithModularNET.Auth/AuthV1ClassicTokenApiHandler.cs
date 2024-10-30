using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Storage;
using MonolithModularNET.Auth.Core;
using MonolithModularNET.Extensions.Abstractions;

namespace MonolithModularNET.Auth;

public class AuthV1ClassicTokenApiHandler
{
    public async Task<IResult> CreateAsync(AuthV1ClassicTokenCreateRequest request, IAuthWriteableRepository<AuthV1ClassicToken> repository, IUnitOfWork<AuthDbContext, IDbContextTransaction> unitOfWork)
    {
        var newToken = new AuthV1ClassicToken()
        {
            Token = Guid.NewGuid().ToString(),
            Description = request.Description,
            Metadata = new AuthV1ClassicTokenMetadata()
            {
                ExpiredAt = request.ExpiredAt,
                IsActive = true
            }
        };

        await repository.AddAsync(newToken);

        await unitOfWork.SaveChangesAsync();

        return Results.Ok(AuthResponse.Success(newToken));
    }

    public Task<IResult> GetAsync()
    {
        throw new NotImplementedException();
    }

    public Task<IResult> DeleteAsync()
    {
        throw new NotImplementedException();
    }
}