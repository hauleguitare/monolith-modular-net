using MonolithModularNET.Extensions.Abstractions;

namespace MonolithModularNET.Extensions.Core.Master;

public partial class CompanyTaskSessionProcess: BaseEntity<int>
{
    public int? CompanyTaskSessionId { get; set; }

    public int StatusId { get; set; }

    public DateTime? ActivatedAt { get; set; }

    public virtual CompanyTaskSession? CompanyTaskSession { get; set; }
}