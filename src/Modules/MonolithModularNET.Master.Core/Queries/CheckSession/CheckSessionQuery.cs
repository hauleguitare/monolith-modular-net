using MonolithModularNET.Extensions.Shared.Cqrs;

namespace MonolithModularNET.Master.Core.Queries.CheckSession;

public class CheckSessionQuery: CqrsQuery<bool>
{
    public CheckSessionQuery(string email)
    {
        Email = email;
    }

    public string Email { get; set; }
}