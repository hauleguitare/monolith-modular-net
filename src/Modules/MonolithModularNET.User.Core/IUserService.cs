using MonolithModularNET.Extensions.Shared.Models;

namespace MonolithModularNET.User.Core;

public interface IUserService
{
    Task<UserResult<UserResponse?>> GetById(string userId);
}