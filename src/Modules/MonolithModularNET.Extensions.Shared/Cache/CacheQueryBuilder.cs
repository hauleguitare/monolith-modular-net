namespace MonolithModularNET.Extensions.Shared.Cache;

public class CacheQueryBuilder
{
    private readonly ICollection<string> _queries = new List<string>();

    public CacheQueryBuilder Append(string query)
    {
        _queries.Add(query);

        return this;
    }

    public override string ToString()
    {
        return string.Join(":", _queries);
    }
}