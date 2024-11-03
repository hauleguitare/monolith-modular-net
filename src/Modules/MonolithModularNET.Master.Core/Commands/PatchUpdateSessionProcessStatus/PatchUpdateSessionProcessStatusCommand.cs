using MonolithModularNET.Extensions.Shared.Cqrs;
using MonolithModularNET.Master.Core.Models;

namespace MonolithModularNET.Master.Core.Commands.PatchUpdateSessionProcessStatus;

public class PatchUpdateSessionProcessStatusCommand: CqrsCommand<CompanyTaskSessionResponse>
{
    public string Email { get; set; } = null!;
    public int StatusId { get; set; }
    
    public DateTime ActivatedAt { get; set; }
}