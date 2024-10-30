using MonolithModularNET.Extensions.Abstractions;
using MonolithModularNET.Extensions.Shared.Repository;
using MonolithModularNET.System.Core;

namespace MonolithModularNET.System.Infrastructure;

public class SystemReadonlyRepository<TEntity>(SystemDbContext context) : ReadonlyEntityRepository<SystemDbContext, TEntity>(context) where TEntity : class, IAggregateRoot, ISystemReadonlyRepository<TEntity>
{
    
}