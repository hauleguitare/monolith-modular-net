using MonolithModularNET.Extensions.Abstractions;
using MonolithModularNET.Extensions.Shared.Repository;
using MonolithModularNET.User.Core;

namespace MonolithModularNET.User;

public class UserWriteableRepository<TEntity>(UserDbContext dbContext) : WriteableEntityRepository<UserDbContext, TEntity>(dbContext) where TEntity : class, IAggregateRoot, IUserWriteableRepository<TEntity>;