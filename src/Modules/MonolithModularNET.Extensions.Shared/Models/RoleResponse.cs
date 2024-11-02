namespace MonolithModularNET.Extensions.Shared.Models;

public class RoleResponse
{
    public string Id { get; set; } = null!;
    public string? Name { get; set; }
    public string? NormalizedName { get; set; }
    
    public int Priority { get; set; }
    
    public bool IsDefault { get; set; }
    public ICollection<string> Permissions { get; set; } = new List<string>();
}