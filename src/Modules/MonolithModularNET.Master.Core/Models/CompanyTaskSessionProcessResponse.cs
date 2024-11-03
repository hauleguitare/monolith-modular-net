namespace MonolithModularNET.Master.Core.Models;

public class CompanyTaskSessionProcessResponse
{
    public int? CompanyTaskSessionId { get; set; }

    public int StatusId { get; set; }
    public string? StatusName { get; set; }
    
    public string? StatusHexColor { get; set; }

    public DateTime? ActivatedAt { get; set; }
}