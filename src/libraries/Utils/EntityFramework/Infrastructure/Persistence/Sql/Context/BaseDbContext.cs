using Microsoft.EntityFrameworkCore;

namespace Utils.EntityFramework.Infrastructure.Persistence.Sql.Context;

public abstract class BaseDbContext<TContext>(DbContextOptions<TContext> options) : DbContext(options) where TContext : BaseDbContext<TContext>
{
    protected abstract string Schema { get; }

    protected virtual void ApplyConfigurations(ModelBuilder modelBuilder)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.HasDefaultSchema(Schema.ToLowerInvariant());
        ApplyConfigurations(modelBuilder);
    }
}
