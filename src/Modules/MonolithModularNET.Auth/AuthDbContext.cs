using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MonolithModularNET.Auth.Core;
using MonolithModularNET.Extensions.Abstractions;

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

        modelBuilder.HasSequence<int>("auth_v1_classic_token_claim_id_seq").IncrementsBy(1);

        modelBuilder.Entity<AuthV1ClassicTokenClaim>(builder =>
        {
            builder.HasKey(e => e.Id).HasName("AuthV1ClassicTokenClaim_pk");

            builder.Property(e => e.Id).HasDefaultValueSql("nextval('auth_v1_classic_token_claim_id_seq'::regclass)");
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
            new()
            {
                Id = 1,
                RoleId = "owner",
                ClaimValue = "v1_classic_token:create",
                ClaimType = AuthClaimTypes.Permission
            },
            new()
            {
                Id = 2,
                RoleId = "owner",
                ClaimValue = "v1_classic_token:update",
                ClaimType = AuthClaimTypes.Permission
            },
            new()
            {
                Id = 3,
                RoleId = "owner",
                ClaimValue = "v1_classic_token:delete",
                ClaimType = AuthClaimTypes.Permission
            },
            new()
            {
                Id = 4,
                RoleId = "owner",
                ClaimValue = "v1_classic_token:view_all",
                ClaimType = AuthClaimTypes.Permission
            }
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
                Priority = 0
            },
            new()
            {
                Id = "super_administrator",
                Name = "Super Administrator",
                NormalizedName = "Super Administrator".ToUpper(),
                ConcurrencyStamp = Guid.NewGuid().ToString(),
                Priority = 1
            },
            new()
            {
                Id = "moderator",
                Name = "Moderator",
                NormalizedName = "Moderator".ToUpper(),
                ConcurrencyStamp = Guid.NewGuid().ToString(),
                Priority = 2
            },
            new()
            {
                Id = "new_user",
                Name = "New User",
                NormalizedName = "New User".ToUpper(),
                ConcurrencyStamp = Guid.NewGuid().ToString(),
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