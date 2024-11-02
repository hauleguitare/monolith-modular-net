using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MonolithModularNET.Extensions.Abstractions;

namespace MonolithModularNET.Extensions.Shared.Extensions;

public static class IsAuditableExtension
{
    public static EntityTypeBuilder IsAuditable<TEntity>(this EntityTypeBuilder<TEntity> modelBuilder) where TEntity : class, IAuditableEntity
    {
        modelBuilder.Property(p => p.CreatedBy).HasColumnType("text");
        
        modelBuilder.Property(p => p.CreatedAt).HasColumnType("datetime");
        
        modelBuilder.Property(p => p.ModifiedBy).HasColumnType("text");
        
        modelBuilder.Property(p => p.ModifiedAt).HasColumnType("datetime");

        return modelBuilder;
    }
}