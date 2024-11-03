using MonolithModularNET.Extensions.Shared.Cqrs;
using MonolithModularNET.Extensions.Shared.Models;

namespace MonolithModularNET.User.Core.Queries.GetRoles;

public class GetRolesQuery: CqrsQuery<ICollection<RoleResponse>>
{
    
}