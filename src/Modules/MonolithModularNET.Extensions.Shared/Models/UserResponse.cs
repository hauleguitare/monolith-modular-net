namespace MonolithModularNET.Extensions.Shared.Models;

public class UserResponse
{
    public string Id { get; set; } = null!;
    
    public string UserName { get; set; } = null!;
    
    public string? FirstName { get; set; }
    
    public string? LastName { get; set; }
    
    public string Email { get; set; } = null!;
    
    public bool EmailConfirmed { get; set; }
    
    public string? PhoneNumber { get; set; }
    
    public bool PhoneNumberConfirmed { get; set; }
    
    public string? AvatarUrl { get; set; }
    
    public bool IsActive { get; set; }
}