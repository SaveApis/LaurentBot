namespace Backend.Domains.Role.Application.Models.HTTP.Responses;

public class RoleResponse
{
    public required Guid Id { get; init; }
    public required DateTime CreatedAt { get; init; }
    public DateTime? UpdatedAt { get; init; }
    public required uint Version { get; init; }

    public required string Key { get; init; }
    public required string Name { get; init; }
    public required string Description { get; init; }
}
