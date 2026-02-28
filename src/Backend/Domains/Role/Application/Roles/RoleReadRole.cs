using Backend.Domains.Role.Infrastructure.Roles;

namespace Backend.Domains.Role.Application.Roles;

public class RoleReadRole : IRole
{
    public string Key => "ROLE_READ";
    public string Name => "Rollen lesen";
    public string Description => "Erlaubt das Lesen von Rollen.";
}
