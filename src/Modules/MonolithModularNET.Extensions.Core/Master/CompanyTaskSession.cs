using MonolithModularNET.Extensions.Abstractions;

namespace MonolithModularNET.Extensions.Core.Master;

public class CompanyTaskSession: AuditableEntity<int>, IAggregateRoot
{
    public string? Email { get; set; }

    public string? Password { get; set; }

    public int? CompanyTaskId { get; set; }

    public virtual CompanyTask? CompanyTask { get; set; }

    public virtual ICollection<CompanyTaskSessionProcess> CompanyTaskSessionProcesses { get; set; } = new List<CompanyTaskSessionProcess>();
}
