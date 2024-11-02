using MonolithModularNET.Extensions.Shared.Cqrs;
using MonolithModularNET.Extensions.Shared.Models;

namespace MonolithModularNET.User.Core.Queries.GetUserById;

public class GetUserByIdQuery(string userId) : CqrsQuery<UserResponse?>
{
    public string UserId { get; set; } = userId;
}