using MonolithModularNET.Extensions.Abstractions;

namespace MonolithModularNET.Master.Core.Repositories;

public interface IMasterWriteableRepository<TEntity>: IWriteableEntityRepository<TEntity> where TEntity : class, IAggregateRoot
{
    
}