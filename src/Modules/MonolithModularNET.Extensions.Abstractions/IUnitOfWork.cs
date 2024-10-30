namespace MonolithModularNET.Extensions.Abstractions;

public interface IUnitOfWork<TContext, TDbContextTransaction> : IDisposable where TContext : class where TDbContextTransaction: class
{
    public TContext Context { get; set; }
    
    public bool SaveChanges();
    public Task<bool> SaveChangesAsync(CancellationToken cancellationToken = default);

    public TDbContextTransaction BeginTransaction();
    
    public Task<TDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default);
    
    public void Commit();
    public Task CommitAsync(CancellationToken cancellationToken = default);

    public void Rollback();

    public Task RollbackAsync(CancellationToken cancellationToken = default);
}
