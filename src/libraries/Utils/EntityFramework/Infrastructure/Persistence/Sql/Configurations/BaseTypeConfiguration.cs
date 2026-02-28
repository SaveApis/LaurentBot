using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Utils.EntityFramework.Infrastructure.Persistence.Sql.Entities;

namespace Utils.EntityFramework.Infrastructure.Persistence.Sql.Configurations;

public abstract class BaseTypeConfiguration<TEntity> : IEntityTypeConfiguration<TEntity> where TEntity : class, IEntity
{
    protected abstract string TableName { get; }

    protected abstract void ConfigureProperties(EntityTypeBuilder<TEntity> builder);
    protected abstract void ConfigureIndexes(EntityTypeBuilder<TEntity> builder);
    protected abstract void ConfigureRelationships(EntityTypeBuilder<TEntity> builder);

    public void Configure(EntityTypeBuilder<TEntity> builder)
    {
        builder.ToTable(TableName.ToLowerInvariant().Replace(' ', '_'));
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id).HasColumnName("id").IsRequired();
        builder.Property(x => x.CreatedAt).HasColumnName("created_at").IsRequired();
        builder.Property(x => x.UpdatedAt).HasColumnName("updated_at").IsRequired();
        builder.Property(x => x.Version).HasColumnName("version").IsRequired();
        ConfigureProperties(builder);
        ConfigureIndexes(builder);
        ConfigureRelationships(builder);
    }
}
