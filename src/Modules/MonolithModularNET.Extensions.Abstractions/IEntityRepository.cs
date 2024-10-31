namespace MonolithModularNET.Extensions.Abstractions;

public interface IReadonlyEntityRepository<TEntity> : IRepository where TEntity : class, IAggregateRoot
{
    public IQueryable<TEntity> AsQueryable();

    public IQueryable<TEntity> AsNoTracking();
    
    public TEntity? FindById(params object?[]? keyValues);
    
    public Task<TEntity?> FindByIdAsync(params object?[]? keyValues);
}

public interface IWriteableEntityRepository<TEntity> : IRepository where TEntity : class, IAggregateRoot
{
    void Add(TEntity entity);
    
    Task AddAsync(TEntity entity, CancellationToken cancellationToken = default);
    
    void AddRange(ICollection<TEntity> entities);
    
    Task AddRangeAsync(ICollection<TEntity> entities, CancellationToken cancellationToken = default);
    
    void UpdateRange(ICollection<TEntity> entities);
    
    void RemoveRange(ICollection<TEntity> entities);
    
    void Update(TEntity entity);
    
    void Remove(TEntity entity);
    
    void Remove(params object?[]? keyValues);
}


public interface IEntityRepository<TEntity>: IReadonlyEntityRepository<TEntity>, IWriteableEntityRepository<TEntity> where TEntity : class, IAggregateRoot;