using MonolithModularNET.Extensions.Abstractions;
using MonolithModularNET.Extensions.Shared.Cqrs;

namespace MonolithModularNET.Master.Core;

public class MasterErrorDescriber
{
    public IError TaskSessionNotFound()
    {
        return new CqrsError()
        {
            Code = nameof(TaskSessionNotFound),
            Description = nameof(TaskSessionNotFound)
        };
    }
}