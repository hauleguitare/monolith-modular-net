using MonolithModularNET.Extensions.Abstractions;
using MonolithModularNET.Extensions.Shared.Repository;
using MonolithModularNET.User.Core;

namespace MonolithModularNET.User;

public class UserReadonlyRepository<TEntity>(UserContext context) : ReadonlyEntityRepository<UserContext, TEntity>(context) where TEntity : class, IAggregateRoot, IUserReadonlyRepository<TEntity>;