using Microsoft.EntityFrameworkCore;
using MonolithModularNET.Extensions.Abstractions;

namespace MonolithModularNET.Extensions.Shared.Repository;

public abstract class ReadonlyEntityRepository<TContext, TEntity>(TContext context)
    : IReadonlyEntityRepository<TEntity>
    where TEntity : class, IAggregateRoot
    where TContext : DbContext
{

    private TContext Context { get; } = context;
    private DbSet<TEntity> DbSet { get; } = context.Set<TEntity>();

    private bool _disposed = false;
    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                Context.Dispose();
            }
        }
        _disposed = true;
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
    

    public virtual IQueryable<TEntity> AsQueryable()
    {
        return DbSet.AsQueryable();
    }

    public virtual TEntity? FindById(params object?[]? keyValues)
    {
        return DbSet.Find(keyValues);
    }

    public virtual Task<TEntity?> FindByIdAsync(params object?[]? keyValues)
    {
        return DbSet.FindAsync(keyValues).AsTask();
    }
}