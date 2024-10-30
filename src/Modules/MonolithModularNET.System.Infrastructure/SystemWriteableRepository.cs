using MonolithModularNET.Extensions.Abstractions;
using MonolithModularNET.Extensions.Shared.Repository;
using MonolithModularNET.System.Core;

namespace MonolithModularNET.System.Infrastructure;

public class SystemWriteableRepository<TEntity>(SystemDbContext context) : WriteableEntityRepository<SystemDbContext, TEntity>(context), ISystemWriteableRepository<TEntity> where TEntity : class, IAggregateRoot
{
    
}