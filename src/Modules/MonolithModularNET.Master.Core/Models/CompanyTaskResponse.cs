namespace MonolithModularNET.Master.Core.Models;

public class CompanyTaskResponse
{
    public int Id { get; set; }
    
    public int? CompanyId { get; set; }

    public string? TaskName { get; set; }

    public string? Description { get; set; }

    public string? Type { get; set; }
    
    public string? CompanyName { get; set; }

    public ICollection<CompanyTaskSessionResponse> Sessions { get; set; } = new List<CompanyTaskSessionResponse>();
}