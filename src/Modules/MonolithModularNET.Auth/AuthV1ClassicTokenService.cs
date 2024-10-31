using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using MonolithModularNET.Auth.Core;
using MonolithModularNET.Extensions.Abstractions;
using MonolithModularNET.Extensions.Shared.Services;

namespace MonolithModularNET.Auth;

public class AuthV1ClassicTokenService(
    IMapper mapper,
    IAuthWriteableRepository<AuthV1ClassicToken> writeableRepository,
    IUnitOfWork<AuthDbContext, IDbContextTransaction> unitOfWork, ICurrentUserService currentUserService, IAuthReadonlyRepository<AuthV1ClassicToken> readonlyRepository)
    : IAuthV1ClassicTokenService
{
    public async Task<AuthResult<AuthV1ClassicTokenResponse>> CreateAsync(AuthV1ClassicTokenCreateRequest request)
    {
        var newToken = mapper.Map<AuthV1ClassicToken>(request);
        
        await writeableRepository.AddAsync(newToken);
        
        await unitOfWork.SaveChangesAsync();
        
        var response = mapper.Map<AuthV1ClassicTokenResponse>(newToken);

        return AuthResult<AuthV1ClassicTokenResponse>.Success(response);
    }

    public async Task<AuthResult<ICollection<AuthV1ClassicTokenResponse>>> GetAsync()
    {
        var describer = new AuthErrorDescriber();
        
        if (!currentUserService.IsAuthenticated)
        {
            return AuthResult<ICollection<AuthV1ClassicTokenResponse>>.Failure([describer.NotAuthenticated()]);
        }

        var userId = currentUserService.UserId;
        
        var tokens = await readonlyRepository.AsNoTracking()
            .Where(e => e.CreatedBy == userId)
            .ProjectTo<AuthV1ClassicTokenResponse>(mapper.ConfigurationProvider).ToListAsync();

        return AuthResult<ICollection<AuthV1ClassicTokenResponse>>.Success(tokens);
    }

    public async Task<AuthResult> DeleteAsync(string tokenId)
    {
        writeableRepository.Remove(tokenId);

        await unitOfWork.SaveChangesAsync();

        return AuthResult.Success();
    }
}