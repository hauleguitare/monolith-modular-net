using MonolithModularNET.Extensions.Shared.Cqrs;
using MonolithModularNET.Extensions.Shared.Models;

namespace MonolithModularNET.User.Core.Queries.GetUsers;

public class GetUsersQuery: CqrsQuery<ICollection<UserResponse>>
{
    
}