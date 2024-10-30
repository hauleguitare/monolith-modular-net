using MonolithModularNET.Auth.Core;
using MonolithModularNET.Extensions.Abstractions;
using MonolithModularNET.Extensions.Shared.Repository;

namespace MonolithModularNET.Auth;

public class AuthReadonlyRepository<TEntity>(AuthDbContext context)
    : ReadonlyEntityRepository<AuthDbContext, TEntity>(context), IAuthReadonlyRepository<TEntity> where TEntity : class, IAggregateRoot;

public class AuthWriteableRepository<TEntity>(AuthDbContext context) : WriteableEntityRepository<AuthDbContext, TEntity>(context), IAuthWriteableRepository<TEntity> where TEntity : class, IAggregateRoot;