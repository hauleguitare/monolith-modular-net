using MonolithModularNET.Extensions.Shared.Cqrs;
using MonolithModularNET.Extensions.Shared.Models;

namespace MonolithModularNET.User.Core.Commands.UpdateRole;

public class UpdateRoleCommand: CqrsCommand<RoleResponse?>
{
    public string Name { get; set; } = null!;
    public int Priority { get; set; }
    public bool IsDefault { get; set; }

    public ICollection<string> Permissions { get; set; } = new List<string>();
}