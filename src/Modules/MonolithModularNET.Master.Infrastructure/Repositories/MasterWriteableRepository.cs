using MonolithModularNET.Extensions.Abstractions;
using MonolithModularNET.Extensions.Shared.Repository;
using MonolithModularNET.Master.Core.Repositories;
using MonolithModularNET.Master.Infrastructure.Context;

namespace MonolithModularNET.Master.Infrastructure.Repositories;

public class MasterWritableRepository<TEntity>(MasterDbContext dbContext) : WriteableEntityRepository<MasterDbContext, TEntity>(dbContext), IMasterWriteableRepository<TEntity> where TEntity : class, IAggregateRoot
{
    
}