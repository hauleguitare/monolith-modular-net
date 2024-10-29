using System.Security.Claims;
using MonolithModularNET.Auth.Core;
using Newtonsoft.Json;

namespace MonolithModularNET.Auth;

public class CacheAuthRole: AuthRole
{
    public ICollection<CacheAuthClaim> Claims { get; set; } = new List<CacheAuthClaim>();
}

public class CacheAuthClaim
{
    [JsonProperty("issuer")]
    public string? Issuer { get; set; }

    [JsonProperty("originalIssuer")]
    public string? OriginalIssuer { get; set; }

    [JsonProperty("properties")]
    public object? Properties { get; set; }

    [JsonProperty("subject")]
    public object? Subject { get; set; }

    [JsonProperty("type")]
    public string? Type { get; set; }

    [JsonProperty("value")]
    public string? Value { get; set; }

    [JsonProperty("valueType")]
    public string? ValueType { get; set; }
}