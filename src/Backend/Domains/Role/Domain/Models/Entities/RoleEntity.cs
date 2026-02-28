using Utils.EntityFramework.Infrastructure.Persistence.Sql.Entities;

namespace Backend.Domains.Role.Domain.Models.Entities;

public class RoleEntity : IEntity
{
    private RoleEntity(Guid id, DateTime createdAt, DateTime? updatedAt, uint version, string key, string name, string description)
    {
        Id = id;
        CreatedAt = createdAt;
        UpdatedAt = updatedAt;
        Version = version;
        Name = name;
        Description = description;
        Key = key;
    }

    public Guid Id { get; }
    public DateTime CreatedAt { get; }
    public DateTime? UpdatedAt { get; private set; }
    public uint Version { get; private set; }

    public string Key { get; }
    public string Name { get; private set; }
    public string Description { get; private set; }

    private bool UpdateName(string name)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);

        if (Name == name)
        {
            return false;
        }

        Name = name;
        return true;
    }
    private bool UpdateDescription(string description)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(description);

        if (Description == description)
        {
            return false;
        }

        Description = description;
        return true;
    }

    public bool Update(string name, string description)
    {
        var changed = false;
        
        changed |= UpdateName(name);
        changed |= UpdateDescription(description);

        if (changed)
        {
            UpdatedAt = DateTime.UtcNow;
            Version++;
        }
        
        return changed;
    }

    public static RoleEntity Create(string key, string name, string description)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(key);
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        ArgumentException.ThrowIfNullOrWhiteSpace(description);

        return new RoleEntity(Guid.Empty, DateTime.UtcNow, null, 1, key, name, description);
    }
}
