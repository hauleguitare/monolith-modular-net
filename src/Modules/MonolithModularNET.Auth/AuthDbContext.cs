using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MonolithModularNET.Auth.Core;

namespace MonolithModularNET.Auth;

public class AuthDbContext(DbContextOptions<AuthDbContext> options)
    : IdentityDbContext<AuthUser, AuthRole, string>(options);