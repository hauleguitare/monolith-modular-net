namespace MonolithModularNET.Auth.Core;

public record SignUpRequest
{
    public SignUpRequest(string email, string password)
    {
        Email = email;
        Password = password;
    }

    public string Email { get; set; }
    public string Password { get; set; }
}