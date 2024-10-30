using MonolithModularNET.Extensions.Shared.UnitOfWork;

namespace MonolithModularNET.System.Infrastructure;

public class SystemUnitOfWork(SystemDbContext context) : UnitOfWork<SystemDbContext>(context);