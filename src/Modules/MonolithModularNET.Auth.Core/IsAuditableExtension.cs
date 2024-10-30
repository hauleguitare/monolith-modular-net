using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MonolithModularNET.Extensions.Abstractions;

namespace MonolithModularNET.Auth.Core;

public static class IsAuditableExtension
{
    public static EntityTypeBuilder IsAuditable<TEntity>(this EntityTypeBuilder<TEntity> modelBuilder) where TEntity : class, IAuditableEntity
    {
        modelBuilder.Property(p => p.CreatedBy).HasColumnType("text");
        
        modelBuilder.Property(p => p.CreatedAt).HasColumnType("timestamp with time zone");
        
        modelBuilder.Property(p => p.ModifiedBy).HasColumnType("text");
        
        modelBuilder.Property(p => p.ModifiedAt).HasColumnType("timestamp with time zone");

        return modelBuilder;
    }
}