using MonolithModularNET.Extensions.Abstractions;
using MonolithModularNET.Extensions.Shared.Repository;
using MonolithModularNET.User.Core;

namespace MonolithModularNET.User;

public class UserWriteableRepository<TEntity>(UserContext context) : WriteableEntityRepository<UserContext, TEntity>(context) where TEntity : class, IAggregateRoot, IWriteableRepository<TEntity>;