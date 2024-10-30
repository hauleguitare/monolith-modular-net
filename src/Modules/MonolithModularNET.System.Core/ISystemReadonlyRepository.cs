using MonolithModularNET.Extensions.Abstractions;

namespace MonolithModularNET.System.Core;

public interface ISystemReadonlyRepository<TEntity> : IReadonlyEntityRepository<TEntity>
    where TEntity : class, IAggregateRoot;
