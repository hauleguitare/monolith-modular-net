using MonolithModularNET.Extensions.Shared.Cqrs;
using MonolithModularNET.Extensions.Shared.Models;

namespace MonolithModularNET.User.Core.Commands.PatchUpdateUser;

public class PatchUpdateUserCommand: CqrsCommand<UserResponse?>
{
    public PatchUpdateUserCommand(string id, string email, string firstName, string lastName, bool isActive, string? avatarUrl)
    {
        Id = id;
        Email = email;
        FirstName = firstName;
        LastName = lastName;
        IsActive = isActive;
        AvatarUrl = avatarUrl;
    }
    
    public string Id { get; set; }

    public string Email { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? PhoneNumber { get; set; }
    public bool IsActive { get; set; }
    
    public string? AvatarUrl { get; set; }
}