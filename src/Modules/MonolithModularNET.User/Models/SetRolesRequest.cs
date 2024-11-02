namespace MonolithModularNET.User.Models;

public class SetRolesRequest
{
    public ICollection<string> RoleNames { get; set; } = new List<string>();
}