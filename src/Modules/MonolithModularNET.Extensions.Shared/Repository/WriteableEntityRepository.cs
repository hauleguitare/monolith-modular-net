using Microsoft.EntityFrameworkCore;
using MonolithModularNET.Extensions.Abstractions;

namespace MonolithModularNET.Extensions.Shared.Repository;

public abstract class WriteableEntityRepository<TContext, TEntity>(TContext context): IWriteableEntityRepository<TEntity>
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

    public void Add(TEntity entity)
    {
        DbSet.Add(entity);
    }

    public Task AddAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        return DbSet.AddAsync(entity, cancellationToken).AsTask();
    }

    public void AddRange(ICollection<TEntity> entities)
    {
        DbSet.AddRange(entities);
    }

    public Task AddRangeAsync(ICollection<TEntity> entities, CancellationToken cancellationToken = default)
    {
        return DbSet.AddRangeAsync(entities, cancellationToken);
    }

    public void UpdateRange(ICollection<TEntity> entities)
    {
        DbSet.UpdateRange(entities);
    }

    public void RemoveRange(ICollection<TEntity> entities)
    {
        DbSet.RemoveRange(entities);
    }

    public void Update(TEntity entity)
    {
        DbSet.Update(entity);
    }

    public void Remove(TEntity entity)
    {
        DbSet.Remove(entity);
    }

    public void Remove(params object?[]? keyValues)
    {
        var entity = DbSet.Find(keyValues);
        if (entity is null)
        {
            return;
        }
        
        DbSet.Remove(entity);
    }
}