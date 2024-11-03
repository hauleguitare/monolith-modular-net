using MonolithModularNET.Extensions.Shared.Cqrs;
using MonolithModularNET.Extensions.Shared.Models;

namespace MonolithModularNET.User.Core.Queries.GetRoleById;

public class GetRoleByIdQuery(string roleId) : CqrsQuery<RoleResponse?>
{
    public string RoleId { get; set; } = roleId;
}