using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using MonolithModularNET.Extensions.Abstractions;
using MonolithModularNET.Extensions.Core.Master;
using MonolithModularNET.Extensions.Shared.Extensions;

namespace MonolithModularNET.Master.Infrastructure.Context;

public class MasterDbContext: DbContext
{
    public MasterDbContext(DbContextOptions<MasterDbContext> options, IHttpContextAccessor httpContextAccessor)
        : base(options)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    private IHttpContextAccessor _httpContextAccessor;
    
    public virtual DbSet<Company> Companies { get; set; }

    public virtual DbSet<CompanyTask> CompanyTasks { get; set; }

    public virtual DbSet<CompanyTaskSession> CompanyTaskSessions { get; set; }

    public virtual DbSet<CompanyTaskSessionProcess> CompanyTaskSessionProcesses { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Company>(entity =>
        {
            
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.HasIndex(e => e.CompanyName, "CompanyName").IsUnique();

            entity.Property(e => e.Id).HasColumnType("int(11)");
            entity.Property(e => e.Description).HasColumnType("text");
            entity.Property(e => e.LogoUrl).HasColumnType("text");
        });

        modelBuilder.Entity<CompanyTask>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.HasIndex(e => e.CompanyId, "CompanyTasks_Company_fk");

            entity.Property(e => e.Id).HasColumnType("int(11)");
            entity.Property(e => e.CompanyId).HasColumnType("int(11)");
            entity.Property(e => e.Description).HasColumnType("text");
            entity.Property(e => e.TaskName).HasColumnType("text");
            entity.Property(e => e.Type).HasColumnType("text");

            entity.HasOne(d => d.Company).WithMany(p => p.CompanyTasks)
                .HasForeignKey(d => d.CompanyId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("CompanyTasks_Company_fk");
        });

        modelBuilder.Entity<CompanyTaskSession>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.HasIndex(e => e.CompanyTaskId, "CompanyTaskSession_CompanyTask_fk");

            entity.Property(e => e.Id).HasColumnType("int(11)");
            entity.Property(e => e.CompanyTaskId).HasColumnType("int(11)");
            entity.Property(e => e.Email).HasMaxLength(255);
            entity.Property(e => e.Password).HasMaxLength(255);

            entity.IsAuditable();

            entity.HasOne(d => d.CompanyTask).WithMany(p => p.CompanyTaskSessions)
                .HasForeignKey(d => d.CompanyTaskId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("CompanyTaskSession_CompanyTask_fk");
        });

        modelBuilder.Entity<CompanyTaskSessionProcess>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.HasIndex(e => e.CompanyTaskSessionId, "CompanyTaskSessionProcess_CompanyTaskSession_fk");

            entity.Property(e => e.Id).HasColumnType("int(11)");
            entity.Property(e => e.ActivatedAt).HasColumnType("datetime");
            entity.Property(e => e.CompanyTaskSessionId).HasColumnType("int(11)");
            entity.Property(e => e.StatusId).HasColumnType("int(11)");

            entity.HasOne(d => d.CompanyTaskSession).WithMany(p => p.CompanyTaskSessionProcesses)
                .HasForeignKey(d => d.CompanyTaskSessionId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("CompanyTaskSessionProcess_CompanyTaskSession_fk");
        });
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
                        if (_httpContextAccessor.HttpContext is null)
                        {
                            ArgumentNullException.ThrowIfNull(_httpContextAccessor.HttpContext);
                        }


                        if (_httpContextAccessor.HttpContext?.User is null)
                        {
                            ArgumentNullException.ThrowIfNull(_httpContextAccessor.HttpContext?.User);
                        }

                        userId = _httpContextAccessor.HttpContext.User.FindFirst(e => e.Type == ClaimTypes.NameIdentifier)
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

                    if (_httpContextAccessor.HttpContext is null)
                    {
                        ArgumentNullException.ThrowIfNull(_httpContextAccessor.HttpContext);
                    }


                    if (_httpContextAccessor.HttpContext?.User is null)
                    {
                        ArgumentNullException.ThrowIfNull(_httpContextAccessor.HttpContext?.User);
                    }

                    userId = _httpContextAccessor.HttpContext.User.FindFirst(e => e.Type == ClaimTypes.NameIdentifier)
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