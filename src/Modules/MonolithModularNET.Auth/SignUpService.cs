using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MonolithModularNET.Auth.Core;
using MonolithModularNET.Extensions.Abstractions;

namespace MonolithModularNET.Auth;

public class SignUpService(
    UserManager<AuthUser> userManager,
    RoleManager<AuthRole> roleManager,
    IUnitOfWork<AuthDbContext> unitOfWork, AuthDbContext _authDbContext)
    : ISignUpService<AuthUser, AuthRole>
{
    public async Task<AuthResult> SignUpAsync(SignUpRequest request, CancellationToken cancellationToken = default(CancellationToken))
    {
        var describer = new AuthErrorDescriber();
        var transaction = await _authDbContext.Database.BeginTransactionAsync(cancellationToken);
        try
        {
            var defaultRole = await roleManager.Roles.Where(e => e.IsDefault).OrderByDescending(e => e.Priority)
                .FirstOrDefaultAsync(cancellationToken: cancellationToken);

            if (defaultRole is null)
            {
                return AuthResult.Failure([describer.RoleNotFound()]);
            }

            var user = new AuthUser()
            {
                Email = request.Email,
                UserName = request.Email,
                FirstName = request.FirstName,
                LastName = request.LastName
            };

            var identityResult = await userManager.CreateAsync(user, request.Password);

            if (!identityResult.Succeeded)
            {
                var errors = identityResult.Errors.Select(e => new AuthError()
                {
                    Code = e.Code,
                    Description = e.Description
                }).ToArray();
                
                return AuthResult.Failure(errors);
            }
            await unitOfWork.SaveChangesAsync(cancellationToken);
            await userManager.AddToRoleAsync(user, defaultRole.Name!);
            
            await unitOfWork.SaveChangesAsync(cancellationToken);

            await transaction.CommitAsync(cancellationToken);
            
            return AuthResult.Success();
        }
        catch (Exception e)
        {
            await transaction.RollbackAsync(cancellationToken);
            throw;
        }
        

    }

    public void Dispose()
    {
        GC.SuppressFinalize(this);
    }
}