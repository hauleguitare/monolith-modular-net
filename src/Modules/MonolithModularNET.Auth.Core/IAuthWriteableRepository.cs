using MonolithModularNET.Extensions.Abstractions;

namespace MonolithModularNET.Auth.Core;

public interface IAuthWriteableRepository<TEntity> : IWriteableEntityRepository<TEntity> where TEntity : class, IAggregateRoot;