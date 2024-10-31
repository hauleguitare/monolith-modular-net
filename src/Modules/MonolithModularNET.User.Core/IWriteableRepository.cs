using MonolithModularNET.Extensions.Abstractions;

namespace MonolithModularNET.User.Core;

public interface IWriteableRepository<TEntity>: IWriteableEntityRepository<TEntity> where TEntity : class, IAggregateRoot
{
    
}