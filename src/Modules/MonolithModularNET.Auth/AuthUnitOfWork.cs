using Microsoft.EntityFrameworkCore.Storage;
using MonolithModularNET.Extensions.Abstractions;

namespace MonolithModularNET.Auth;

public class AuthUnitOfWork: IUnitOfWork<AuthDbContext>
{
    public AuthDbContext Context { get; set; }
    
    public AuthUnitOfWork(AuthDbContext context)
    {
        Context = context;
    }

    public void Dispose()
    {
        Context.Dispose();
        GC.SuppressFinalize(this);
    }

    public bool SaveChanges()
    {
        return Context.SaveChanges() > 0;
    }


    public async Task<bool> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await Context.SaveChangesAsync(cancellationToken) > 0;
    }

    public void Commit()
    {
        Context.Database.CommitTransaction();
    }

    public async Task CommitAsync(CancellationToken cancellationToken = default)
    {
        await Context.Database.CommitTransactionAsync(cancellationToken);

    }

    public void Rollback()
    {
        Context.Database.RollbackTransaction();
    }

    public async Task RollbackAsync(CancellationToken cancellationToken = default)
    {
        await Context.Database.RollbackTransactionAsync(cancellationToken);
    }
}