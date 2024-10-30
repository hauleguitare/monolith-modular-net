using MonolithModularNET.Extensions.Abstractions;

namespace MonolithModularNET.System.Core;

public interface ISystemWriteableRepository<TEntity>: IWriteableEntityRepository<TEntity> where TEntity : class, IAggregateRoot
{
    
}