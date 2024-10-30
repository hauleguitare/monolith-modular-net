using MonolithModularNET.Extensions.Abstractions;

namespace MonolithModularNET.Auth.Core;

public interface IAuthReadonlyRepository<TEntity>: IReadonlyEntityRepository<TEntity> where TEntity : class, IAggregateRoot;