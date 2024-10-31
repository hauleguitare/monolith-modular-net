using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MonolithModularNET.Auth.Core;

namespace MonolithModularNET.User;

public class UserContext(
    DbContextOptions<UserContext> options,
    IPasswordHasher<AuthUser> passwordHasher,
    IHttpContextAccessor httpContextAccessor)
    : IdentityDbContext<AuthUser, AuthRole, string>(options)
{
    
}