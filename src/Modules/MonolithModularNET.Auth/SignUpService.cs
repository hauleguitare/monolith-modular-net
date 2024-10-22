using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using MonolithModularNET.Auth.Core;
using MonolithModularNET.Extensions.Abstractions;

namespace MonolithModularNET.Auth;

public class SignUpService: ISignUpService<AuthUser, AuthRole>
{
    private readonly UserManager<AuthUser> _userManager;
    private readonly RoleManager<AuthRole> _roleManager;
    private readonly IUnitOfWork<AuthContext, IDbContextTransaction> _unitOfWork;

    public SignUpService(UserManager<AuthUser> userManager, RoleManager<AuthRole> roleManager, IUnitOfWork<AuthContext, IDbContextTransaction> unitOfWork)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _unitOfWork = unitOfWork;
    }

    public async Task<IdentityResult> SignUpAsync(SignUpRequest request, CancellationToken cancellationToken = default(CancellationToken))
    {
        var transaction = await _unitOfWork.BeginTransactionAsync(cancellationToken);
        try
        {
            var defaultRole = await _roleManager.Roles.Where(e => e.IsDefault).OrderByDescending(e => e.Priority)
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

            var identityResult = await _userManager.CreateAsync(user, request.Password);

            if (!identityResult.Succeeded)
            {
                //TODO: Change exception
                throw new Exception("Can't create user, please try again");
            }
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            await _userManager.AddToRoleAsync(user, defaultRole.Name!);
            
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            
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