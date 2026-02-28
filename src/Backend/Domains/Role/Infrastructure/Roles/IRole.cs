namespace Backend.Domains.Role.Infrastructure.Roles;

public interface IRole
{
    string Key { get; }
    string Name { get; }
    string Description { get; }
}
