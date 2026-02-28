namespace Utils.EntityFramework.Infrastructure.Persistence.Sql.Entities;

public interface IEntity
{
    public Guid Id { get; }
    public DateTime CreatedAt { get; }
    public DateTime? UpdatedAt { get; }
    public uint Version { get; }
}
