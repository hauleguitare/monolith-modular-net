using MonolithModularNET.Extensions.Abstractions;
using MonolithModularNET.Extensions.Shared.Repository;
using MonolithModularNET.Master.Core.Repositories;
using MonolithModularNET.Master.Infrastructure.Context;

namespace MonolithModularNET.Master.Infrastructure.Repositories;

public class MasterReadonlyRepository<TEntity>(MasterDbContext dbContext) : ReadonlyEntityRepository<MasterDbContext, TEntity>(dbContext), IMasterReadonlyRepository<TEntity> where TEntity : class, IAggregateRoot
{
    
}