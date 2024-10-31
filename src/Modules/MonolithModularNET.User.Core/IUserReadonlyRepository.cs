using MonolithModularNET.Extensions.Abstractions;

namespace MonolithModularNET.User.Core;

public interface IUserReadonlyRepository<TEntity>: IReadonlyEntityRepository<TEntity> where TEntity : class, IAggregateRoot
{
    
}