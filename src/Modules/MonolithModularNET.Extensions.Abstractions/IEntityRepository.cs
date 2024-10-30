namespace MonolithModularNET.Extensions.Abstractions;

public interface IEntityRepository<TEntity, in TKey>: IRepository where TEntity : IAggregateRoot
{
    public IQueryable<TEntity> AsQueryable();
    public TEntity? GetById(TKey id);
    public Task<TEntity?> GetByIdAsync(TKey id, CancellationToken token = default);
    void Add(TEntity entity);
    Task AddAsync(TEntity entity, CancellationToken cancellationToken = default);
    void AddRange(ICollection<TEntity> entities);
    Task AddRangeAsync(ICollection<TEntity> entities, CancellationToken cancellationToken = default);
    void UpdateRange(ICollection<TEntity> entities);
    void RemoveRange(ICollection<TEntity> entities);
    void Update(TEntity entity);
    void Remove(TEntity entity);
    void Remove(TKey id);
}