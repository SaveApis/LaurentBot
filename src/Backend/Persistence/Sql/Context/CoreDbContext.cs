using Backend.Domains.Role.Domain.Models.Entities;
using Backend.Domains.Role.Persistence.Sql.Configurations;
using Microsoft.EntityFrameworkCore;
using Utils.EntityFramework.Infrastructure.Persistence.Sql.Context;

namespace Backend.Persistence.Sql.Context;

public class CoreDbContext(DbContextOptions<CoreDbContext> options) : BaseDbContext<CoreDbContext>(options)
{
    protected override string Schema => "core";

    public DbSet<RoleEntity> Roles => Set<RoleEntity>();

    protected override void ApplyConfigurations(ModelBuilder modelBuilder)
    {
        base.ApplyConfigurations(modelBuilder);

        modelBuilder.ApplyConfiguration(new RoleEntityTypeConfiguration());
    }
}
