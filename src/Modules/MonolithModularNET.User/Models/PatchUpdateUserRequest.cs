namespace MonolithModularNET.User.Models;

public class PatchUpdateUserRequest
{
    public string Email { get; set; } = null!;
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? PhoneNumber { get; set; }
    public bool IsActive { get; set; }
    public string? AvatarUrl { get; set; }
}