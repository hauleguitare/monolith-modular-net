using MonolithModularNET.Extensions.Shared.Cqrs;
using MonolithModularNET.Extensions.Shared.Models;

namespace MonolithModularNET.User.Core.Commands.SetRoles;

public class SetRolesCommand: CqrsCommand<UserResponse?>
{
    public SetRolesCommand(string userId, ICollection<string> roleNames)
    {
        UserId = userId;
        RoleNames = roleNames;
    }

    public string UserId { get; set; }
    
    public ICollection<string> RoleNames { get; set; }
}