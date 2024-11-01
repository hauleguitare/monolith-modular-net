using MonolithModularNET.Extensions.Abstractions;

namespace MonolithModularNET.User.Core;

public interface IUserWriteableRepository<TEntity>: IWriteableEntityRepository<TEntity> where TEntity : class, IAggregateRoot
{
    
}