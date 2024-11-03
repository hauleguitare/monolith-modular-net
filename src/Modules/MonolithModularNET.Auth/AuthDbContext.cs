using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MonolithModularNET.Auth.Core;
using MonolithModularNET.Extensions.Abstractions;
using MonolithModularNET.Extensions.Shared.Permissions;

namespace MonolithModularNET.Auth;

public class AuthDbContext(
    DbContextOptions<AuthDbContext> options,
    IPasswordHasher<AuthUser> passwordHasher,
    IHttpContextAccessor httpContextAccessor)
    : IdentityDbContext<AuthUser, AuthRole, string>(options)
{
    public DbSet<AuthV1ClassicToken> AuthV1ClassicTokens { get; set; }
    public DbSet<AuthV1ClassicTokenClaim> AuthV1ClassicTokenClaims { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);


        // AuthV1ClassicToken builder
        modelBuilder.Entity<AuthV1ClassicToken>(builder =>
        {
            builder.HasKey(e => e.Token).HasName("AuthV1ClassicToken_pk");

            builder.OwnsOne(e => e.Metadata)
                .Property(p => p.ExpiredAt).HasColumnName("Metadata_ExpiredAt");

            builder.OwnsOne(e => e.Metadata)
                .Property(p => p.IsActive).HasColumnName("Metadata_IsActive").HasDefaultValue(false);

            builder.HasMany(d => d.Claims)
                .WithOne()
                .HasForeignKey(d => d.Token)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("AuthV1ClassicTokenClaim_Token_fk");

            builder.IsAuditable();
        });

        modelBuilder.Entity<AuthV1ClassicTokenClaim>(builder =>
        {
            builder.HasKey(e => e.Id).HasName("AuthV1ClassicTokenClaim_pk");

            builder.Property(e => e.Id).ValueGeneratedOnAdd();
        });

        SeedRootUser(modelBuilder);

        SeedRoles(modelBuilder);

        SeedRoleClaims(modelBuilder);

        SeedUserRole(modelBuilder);
    }

    private ModelBuilder SeedRoleClaims(ModelBuilder modelBuilder)
    {
        var roleClaims = new List<IdentityRoleClaim<string>>()
        {
            // Classic token
            new()
            {
                Id = 1,
                RoleId = "owner",
                ClaimValue = AuthPermissions.V1ClassicTokenCreate,
                ClaimType = AuthClaimTypes.Permission
            },
            new()
            {
                Id = 2,
                RoleId = "owner",
                ClaimValue = AuthPermissions.V1ClassicTokenUpdate,
                ClaimType = AuthClaimTypes.Permission
            },
            new()
            {
                Id = 3,
                RoleId = "owner",
                ClaimValue = AuthPermissions.V1ClassicTokenDelete,
                ClaimType = AuthClaimTypes.Permission
            },
            new()
            {
                Id = 4,
                RoleId = "owner",
                ClaimValue = AuthPermissions.V1ClassicTokenViewAll,
                ClaimType = AuthClaimTypes.Permission
            },
            new()
            {
                Id = 5,
                RoleId = "owner",
                ClaimValue = AuthPermissions.V1ClassicTokenSetRoles,
                ClaimType = AuthClaimTypes.Permission
            },
            
            // User
            new()
            {
                Id = 6,
                RoleId = "owner",
                ClaimValue = UserPermissions.Create,
                ClaimType = AuthClaimTypes.Permission
            },
            new()
            {
                Id = 7,
                RoleId = "owner",
                ClaimValue = UserPermissions.Update,
                ClaimType = AuthClaimTypes.Permission
            },
            new()
            {
                Id = 8,
                RoleId = "owner",
                ClaimValue = UserPermissions.Delete,
                ClaimType = AuthClaimTypes.Permission
            },
            new()
            {
                Id = 9,
                RoleId = "owner",
                ClaimValue = UserPermissions.SetRoles,
                ClaimType = AuthClaimTypes.Permission
            },
            new()
            {
                Id = 10,
                RoleId = "owner",
                ClaimValue = UserPermissions.ViewAll,
                ClaimType = AuthClaimTypes.Permission
            },
            
            // Role
            new()
            {
                Id = 11,
                RoleId = "owner",
                ClaimValue = RolePermissions.Create,
                ClaimType = AuthClaimTypes.Permission
            },
            new()
            {
                Id = 12,
                RoleId = "owner",
                ClaimValue = RolePermissions.Update,
                ClaimType = AuthClaimTypes.Permission
            },
            new()
            {
                Id = 13,
                RoleId = "owner",
                ClaimValue = RolePermissions.Delete,
                ClaimType = AuthClaimTypes.Permission
            },
            new()
            {
                Id = 14,
                RoleId = "owner",
                ClaimValue = RolePermissions.SetPermissions,
                ClaimType = AuthClaimTypes.Permission
            },
            new()
            {
                Id = 15,
                RoleId = "owner",
                ClaimValue = RolePermissions.ViewAll,
                ClaimType = AuthClaimTypes.Permission
            },
        };

        modelBuilder.Entity<IdentityRoleClaim<string>>().HasData(roleClaims);

        return modelBuilder;
    }

    private ModelBuilder SeedRootUser(ModelBuilder modelBuilder)
    {
        var email = "root@root.com";
        var rootAdmin = new AuthUser()
        {
            Id = "root",
            Email = email,
            EmailConfirmed = true,
            IsActive = true,
            FirstName = "Super",
            LastName = "Administrator",
            SecurityStamp = Guid.NewGuid().ToString("N"),
            UserName = email,
            NormalizedUserName = email.ToUpper(),
            NormalizedEmail = email.ToUpper()
        };
        var passwordHashed = passwordHasher.HashPassword(rootAdmin, "123456@#Abc");

        rootAdmin.PasswordHash = passwordHashed;

        modelBuilder.Entity<AuthUser>().HasData(rootAdmin);

        return modelBuilder;
    }

    private ModelBuilder SeedUserRole(ModelBuilder modelBuilder)
    {
        var userRole = new IdentityUserRole<string>()
        {
            UserId = "root",
            RoleId = "owner",
        };

        modelBuilder.Entity<IdentityUserRole<string>>().HasData(userRole);
        return modelBuilder;
    }

    private ModelBuilder SeedRoles(ModelBuilder modelBuilder)
    {
        var roles = new List<AuthRole>()
        {
            new()
            {
                Id = "owner",
                Name = "Owner",
                NormalizedName = "Owner".ToUpper(),
                ConcurrencyStamp = Guid.NewGuid().ToString(),
                IsDefault = true,
                Priority = 0
            },
            new()
            {
                Id = "administrator",
                Name = "Administrator",
                NormalizedName = "Administrator".ToUpper(),
                ConcurrencyStamp = Guid.NewGuid().ToString(),
                IsDefault = true,
                Priority = 1
            },
            new()
            {
                Id = "moderator",
                Name = "Moderator",
                NormalizedName = "Moderator".ToUpper(),
                ConcurrencyStamp = Guid.NewGuid().ToString(),
                IsDefault = true,
                Priority = 2
            },
            new()
            {
                Id = "user",
                Name = "User",
                NormalizedName = "User".ToUpper(),
                ConcurrencyStamp = Guid.NewGuid().ToString(),
                IsDefault = true,
                Priority = 999999
            }
        };

        modelBuilder.Entity<AuthRole>().HasData(roles);

        return modelBuilder;
    }


    private void BeforeSaveChangesAsync()
    {
        var trackEntities = ChangeTracker.Entries<IAuditableEntity>().ToList();
        foreach (var entityEntry in trackEntities)
        {
            string? userId;
            switch (entityEntry.State)
            {
                case EntityState.Added:
                    if (!entityEntry.Entity.CreatedAt.HasValue)
                    {
                        entityEntry.Entity.UpdateCreatedAt();
                    }

                    if (string.IsNullOrEmpty(entityEntry.Entity.CreatedBy))
                    {
                        if (httpContextAccessor.HttpContext is null)
                        {
                            ArgumentNullException.ThrowIfNull(httpContextAccessor.HttpContext);
                        }


                        if (httpContextAccessor.HttpContext?.User is null)
                        {
                            ArgumentNullException.ThrowIfNull(httpContextAccessor.HttpContext?.User);
                        }

                        userId = httpContextAccessor.HttpContext.User.FindFirst(e => e.Type == ClaimTypes.NameIdentifier)
                            ?.Value;

                        if (string.IsNullOrEmpty(userId))
                        {
                            ArgumentNullException.ThrowIfNull(userId);
                        }

                        entityEntry.Entity.AddCreatedBy(userId);
                    }

                    break;
                
                
                case EntityState.Modified:
                    entityEntry.Entity.UpdateModifiedAt();

                    if (httpContextAccessor.HttpContext is null)
                    {
                        ArgumentNullException.ThrowIfNull(httpContextAccessor.HttpContext);
                    }


                    if (httpContextAccessor.HttpContext?.User is null)
                    {
                        ArgumentNullException.ThrowIfNull(httpContextAccessor.HttpContext?.User);
                    }

                    userId = httpContextAccessor.HttpContext.User.FindFirst(e => e.Type == ClaimTypes.NameIdentifier)
                        ?.Value;

                    if (string.IsNullOrEmpty(userId))
                    {
                        ArgumentNullException.ThrowIfNull(userId);
                    }

                    entityEntry.Entity.AddModifiedBy(userId);

                    break;
            }
        }
    }

    public override int SaveChanges(bool acceptAllChangesOnSuccess)
    {
        BeforeSaveChangesAsync();

        return base.SaveChanges(acceptAllChangesOnSuccess);
    }


    public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = new CancellationToken())
    {
        BeforeSaveChangesAsync();
        return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
    {
        BeforeSaveChangesAsync();
        return base.SaveChangesAsync(cancellationToken);
    }
}