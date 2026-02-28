using Backend.Domains.Role.Domain.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Utils.EntityFramework.Infrastructure.Persistence.Sql.Configurations;

namespace Backend.Domains.Role.Persistence.Sql.Configurations;

public class RoleEntityTypeConfiguration : BaseTypeConfiguration<RoleEntity>
{
    protected override string TableName => "roles";

    protected override void ConfigureProperties(EntityTypeBuilder<RoleEntity> builder)
    {
        builder.Property(x => x.Key).IsRequired().HasColumnName("key").HasMaxLength(50);
        builder.Property(x => x.Name).IsRequired().HasColumnName("name").HasMaxLength(100);
        builder.Property(x => x.Description).IsRequired().HasColumnName("description").HasMaxLength(500);
    }

    protected override void ConfigureIndexes(EntityTypeBuilder<RoleEntity> builder)
    {
        builder.HasIndex(x => x.Key).IsUnique();
    }
}
