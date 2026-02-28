using Backend.Domains.Role.Application.Models.DTO;
using Backend.Domains.Role.Application.Models.HTTP.Responses;
using Backend.Domains.Role.Domain.Models.Entities;
using Backend.Domains.Role.Infrastructure.Roles;

namespace Backend.Domains.Role.Application.Mapping;

public interface IRoleMapper
{
    IEnumerable<RoleGetDto> ToDto(IEnumerable<RoleEntity> entities);
    RoleGetDto ToDto(RoleEntity entity);
    RoleCreateDto ToCreateDto(IRole role);
    RoleUpdateDto ToUpdateDto(IRole role);

    IEnumerable<RoleResponse> ToResponse(IEnumerable<RoleGetDto> dtos);
    RoleResponse ToResponse(RoleGetDto dto);
}
