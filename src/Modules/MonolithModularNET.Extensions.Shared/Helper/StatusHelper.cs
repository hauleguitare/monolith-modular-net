namespace MonolithModularNET.Extensions.Shared.Helper;

public class StatusMapping
{
    public StatusMapping(string name, string hexColor)
    {
        Name = name;
        HexColor = hexColor;
    }

    public string Name { get; set; }
    public string HexColor { get; set; }
}


public static class StatusHelper
{
    public static readonly IDictionary<int, StatusMapping> Statuses = new Dictionary<int, StatusMapping>()
    {
        { 0, new StatusMapping("Idle", "#deba07") },
        { 1, new StatusMapping("Processing", "#4287f5") },
        { 2, new StatusMapping("Failed", "#c40616") },
        { 3, new StatusMapping("Succeed", "#06c442") },
        { 4, new StatusMapping("Cancelled", "#c40616") },
    };

    public static string GetHexColor(int id)
    {
        return Statuses.FirstOrDefault(e => e.Key == id).Value.HexColor;
    }
    
    public static string GetName(int id)
    {
        return Statuses.FirstOrDefault(e => e.Key == id).Value.Name;
    }
}