using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using MonolithModularNET.Auth.Core;
using MonolithModularNET.Extensions.Abstractions;

namespace MonolithModularNET.Auth;

public class SignUpService(
    UserManager<AuthUser> userManager,
    RoleManager<AuthRole> roleManager,
    IUnitOfWork<AuthContext, IDbContextTransaction> unitOfWork)
    : ISignUpService<AuthUser, AuthRole>
{
    public async Task<AuthResult> SignUpAsync(SignUpRequest request, CancellationToken cancellationToken = default(CancellationToken))
    {
        var describer = new AuthErrorDescriber();
        await unitOfWork.BeginTransactionAsync(cancellationToken);
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
                UserName = request.Email
            };

            var identityResult = await userManager.CreateAsync(user, request.Password);

            if (!identityResult.Succeeded)
            {
                return AuthResult.Failure(identityResult.Errors.Select(e => new AuthError()
                {
                    Code = e.Code,
                    Description = e.Description
                }).ToList());
            }
            await unitOfWork.SaveChangesAsync(cancellationToken);
            await userManager.AddToRoleAsync(user, defaultRole.Name!);
            
            await unitOfWork.SaveChangesAsync(cancellationToken);
            
            await unitOfWork.CommitAsync(cancellationToken);
            
            return AuthResult.Success();
        }
        catch (Exception e)
        {
            await unitOfWork.RollbackAsync(cancellationToken);
            throw;
        }
        

    }

    public void Dispose()
    {
        GC.SuppressFinalize(this);
    }
}