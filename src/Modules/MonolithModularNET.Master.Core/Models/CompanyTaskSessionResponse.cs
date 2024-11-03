namespace MonolithModularNET.Master.Core.Models;

public class CompanyTaskSessionResponse
{
    public string? Email { get; set; }

    public string? Password { get; set; }

    public int? CompanyTaskId { get; set; }

    public CompanyTaskSessionProcessResponse? Process { get; set; }
}