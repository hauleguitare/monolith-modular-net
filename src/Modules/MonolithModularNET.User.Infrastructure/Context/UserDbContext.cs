using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MonolithModularNET.Auth.Core;

namespace MonolithModularNET.User.Infrastructure.Context;

public class UserDbContext(
    DbContextOptions<UserDbContext> options,
    IHttpContextAccessor httpContextAccessor)
    : IdentityDbContext<AuthUser, AuthRole, string>(options)
{
    
}