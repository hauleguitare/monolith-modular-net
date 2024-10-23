using System.Security.Claims;
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
    public async Task<IdentityResult> SignUpAsync(SignUpRequest request, CancellationToken cancellationToken = default(CancellationToken))
    {
        var transaction = await unitOfWork.BeginTransactionAsync(cancellationToken);
        try
        {
            var defaultRole = await roleManager.Roles.Where(e => e.IsDefault).OrderByDescending(e => e.Priority)
                .FirstOrDefaultAsync(cancellationToken: cancellationToken);

            if (defaultRole is null)
            {
                //TODO: Change exception
                throw new Exception("Not support Default Role");
            }

            var user = new AuthUser()
            {
                Email = request.Email,
                UserName = request.Email
            };

            var identityResult = await userManager.CreateAsync(user, request.Password);

            if (!identityResult.Succeeded)
            {
                //TODO: Change exception
                throw new Exception("Can't create user, please try again");
            }
            await unitOfWork.SaveChangesAsync(cancellationToken);
            await userManager.AddToRoleAsync(user, defaultRole.Name!);
            
            await unitOfWork.SaveChangesAsync(cancellationToken);
            
            await transaction.CommitAsync(cancellationToken);
            return identityResult;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            
            await transaction.RollbackAsync(cancellationToken);
            throw;
        }
        

    }

    public void Dispose()
    {
        GC.SuppressFinalize(this);
    }
}