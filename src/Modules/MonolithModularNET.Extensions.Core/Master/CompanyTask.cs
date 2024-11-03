using MonolithModularNET.Extensions.Abstractions;

namespace MonolithModularNET.Extensions.Core.Master;

public class CompanyTask: BaseEntity<int>, IAggregateRoot
{
    public int? CompanyId { get; set; }

    public string? TaskName { get; set; }

    public string? Description { get; set; }

    public string? Type { get; set; }

    public virtual Company? Company { get; set; }

    public virtual ICollection<CompanyTaskSession> CompanyTaskSessions { get; set; } = new List<CompanyTaskSession>();
}