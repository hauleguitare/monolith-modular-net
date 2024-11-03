namespace MonolithModularNET.Extensions.Abstractions;

public interface IUnitOfWork<TContext> : IUnitOfWork where TContext : class
{
    public TContext Context { get; set; }
}


public interface IUnitOfWork : IDisposable
{
    public bool SaveChanges();
    public Task<bool> SaveChangesAsync(CancellationToken cancellationToken = default);

    public void Commit();
    public Task CommitAsync(CancellationToken cancellationToken = default);

    public void Rollback();

    public Task RollbackAsync(CancellationToken cancellationToken = default);
}