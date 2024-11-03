using MonolithModularNET.Extensions.Shared.Cqrs;
using MonolithModularNET.Master.Core.Models;

namespace MonolithModularNET.Master.Core.Queries.GetCompanyTask;

public class GetCompanyTaskQuery: CqrsQuery<ICollection<CompanyTaskResponse>>
{
    
}