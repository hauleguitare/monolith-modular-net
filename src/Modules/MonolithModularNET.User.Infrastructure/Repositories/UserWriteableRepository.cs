using MonolithModularNET.Extensions.Abstractions;
using MonolithModularNET.Extensions.Shared.Repository;
using MonolithModularNET.User.Core;
using MonolithModularNET.User.Infrastructure.Context;

namespace MonolithModularNET.User.Infrastructure.Repositories;

public class UserWriteableRepository<TEntity>(UserDbContext dbContext) : WriteableEntityRepository<UserDbContext, TEntity>(dbContext) where TEntity : class, IAggregateRoot, IUserWriteableRepository<TEntity>;