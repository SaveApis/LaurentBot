using Backend.Domains.Role.Application.Models.DTO;
using Backend.Domains.Role.Application.Models.HTTP.Responses;
using Backend.Domains.Role.Domain.Models.Entities;
using Backend.Domains.Role.Infrastructure.Roles;

namespace Backend.Domains.Role.Application.Mapping;

public class RoleMapper : IRoleMapper
{
    public IEnumerable<RoleGetDto> ToDto(IEnumerable<RoleEntity> entities)
    {
        return entities.Select(ToDto);
    }

    public RoleGetDto ToDto(RoleEntity entity)
    {
        return new RoleGetDto
        {
            Id = entity.Id,
            CreatedAt = entity.CreatedAt,
            UpdatedAt = entity.UpdatedAt,
            Version = entity.Version,
            Key = entity.Key,
            Name = entity.Name,
            Description = entity.Description,
        };
    }
    public RoleCreateDto ToCreateDto(IRole role)
    {
        return new RoleCreateDto
        {
            Key = role.Key, Name = role.Name, Description = role.Description
        };
    }

    public RoleUpdateDto ToUpdateDto(IRole role)
    {
        return new RoleUpdateDto
        {
            Name = role.Name, Description = role.Description
        };
    }

    public IEnumerable<RoleResponse> ToResponse(IEnumerable<RoleGetDto> dtos)
    {
        return dtos.Select(ToResponse);
    }

    public RoleResponse ToResponse(RoleGetDto dto)
    {
        return new RoleResponse
        {
            Id = dto.Id,
            CreatedAt = dto.CreatedAt,
            UpdatedAt = dto.UpdatedAt,
            Version = dto.Version,
            Key = dto.Key,
            Name = dto.Name,
            Description = dto.Description
        };
    }
}
