using MonolithModularNET.Extensions.Shared.Cqrs;
using MonolithModularNET.Master.Core.Models;

namespace MonolithModularNET.Master.Core.Commands.AddSessionProcess;

public class AddSessionProcessCommand: CqrsCommand<CompanyTaskSessionResponse?>
{
    public AddSessionProcessCommand(int companyTaskId, string email, int statusId)
    {
        CompanyTaskId = companyTaskId;
        Email = email;
        StatusId = statusId;
    }

    public int CompanyTaskId { get; set; }
    public string Email { get; set; }
    public string? Password { get; set; }
    public int StatusId { get; set; }
}