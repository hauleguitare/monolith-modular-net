using MonolithModularNET.Extensions.Shared.Cqrs;
using MonolithModularNET.Extensions.Shared.Models;

namespace MonolithModularNET.User.Core.Commands.UpdateUser;

public class UpdateUserCommand: CqrsCommand<UserResponse?>
{
    public string Email { get; set; } = null!;
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? PhoneNumber { get; set; }
    public bool IsActive { get; set; }
    public string? AvatarUrl { get; set; }
    public ICollection<string> RoleNames { get; set; } = new List<string>();
}