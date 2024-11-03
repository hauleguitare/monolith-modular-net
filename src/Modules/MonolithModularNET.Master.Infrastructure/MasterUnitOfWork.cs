using MonolithModularNET.Extensions.Shared.UnitOfWork;
using MonolithModularNET.Master.Core;
using MonolithModularNET.Master.Infrastructure.Context;

namespace MonolithModularNET.Master.Infrastructure;

public class MasterUnitOfWork(MasterDbContext context) : UnitOfWork<MasterDbContext>(context), IMasterUnitOfWork
{
    
}