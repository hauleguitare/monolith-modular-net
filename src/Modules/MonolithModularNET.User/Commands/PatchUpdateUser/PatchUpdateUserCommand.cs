using MonolithModularNET.Extensions.Shared.Cqrs;
using MonolithModularNET.Extensions.Shared.Models;

namespace MonolithModularNET.User.Commands.PatchUpdateUser;

public class PatchUpdateUserCommand: CqrsCommand<UserResponse?>
{
    public PatchUpdateUserCommand(string email, string firstName, string lastName, bool isActive, string? avatarUrl)
    {
        Email = email;
        FirstName = firstName;
        LastName = lastName;
        IsActive = isActive;
        AvatarUrl = avatarUrl;
    }

    public string Email { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? PhoneNumber { get; set; }
    public bool IsActive { get; set; }
    
    public string? AvatarUrl { get; set; }
}