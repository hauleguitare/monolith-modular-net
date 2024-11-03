using MonolithModularNET.Extensions.Abstractions;

namespace MonolithModularNET.Extensions.Core.Master;

public class Company: BaseEntity<int>
{
    public string? CompanyName { get; set; }

    public string? Description { get; set; }

    public string? LogoUrl { get; set; }

    public virtual ICollection<CompanyTask> CompanyTasks { get; set; } = new List<CompanyTask>();
}
