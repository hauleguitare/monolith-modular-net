namespace MonolithModularNET.Extensions.Abstractions;

public interface IUnitOfWork<TContext> : IDisposable where TContext : class
{
    public TContext Context { get; set; }
    
    public bool SaveChanges();
    public Task<bool> SaveChangesAsync(CancellationToken cancellationToken = default);

    public void Commit();
    public Task CommitAsync(CancellationToken cancellationToken = default);

    public void Rollback();

    public Task RollbackAsync(CancellationToken cancellationToken = default);
}
