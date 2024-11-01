using MonolithModularNET.Extensions.Abstractions;
using MonolithModularNET.Extensions.Shared.Repository;
using MonolithModularNET.User.Core;

namespace MonolithModularNET.User;

public class UserReadonlyRepository<TEntity>(UserDbContext dbContext) : ReadonlyEntityRepository<UserDbContext, TEntity>(dbContext) where TEntity : class, IAggregateRoot, IUserReadonlyRepository<TEntity>;