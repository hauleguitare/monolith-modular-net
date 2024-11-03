using MonolithModularNET.Extensions.Abstractions;

namespace MonolithModularNET.Master.Core.Repositories;

public interface IMasterReadonlyRepository<TEntity>: IReadonlyEntityRepository<TEntity> where TEntity : class, IAggregateRoot
{
    
}