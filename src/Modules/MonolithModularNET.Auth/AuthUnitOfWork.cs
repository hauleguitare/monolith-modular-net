using Microsoft.EntityFrameworkCore.Storage;
using MonolithModularNET.Extensions.Abstractions;

namespace MonolithModularNET.Auth;

public class AuthUnitOfWork: IUnitOfWork<AuthContext, IDbContextTransaction>
{
    public AuthContext Context { get; set; }
    
    public AuthUnitOfWork(AuthContext context)
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

    public IDbContextTransaction BeginTransaction()
    {
        return Context.Database.BeginTransaction();
    }

    public Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default)
    {
        return Context.Database.BeginTransactionAsync(cancellationToken);
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