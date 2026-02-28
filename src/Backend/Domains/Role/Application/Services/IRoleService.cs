using Backend.Domains.Role.Application.Models.DTO;

namespace Backend.Domains.Role.Application.Services;

public interface IRoleService
{
    Task<IReadOnlyList<RoleGetDto>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<RoleGetDto> GetByKeyAsync(string key, CancellationToken cancellationToken = default);

    Task<RoleGetDto> CreateAsync(RoleCreateDto dto, CancellationToken cancellationToken = default);
    Task<RoleGetDto> UpdateAsync(string key, RoleUpdateDto dto, CancellationToken cancellationToken = default);
    Task DeleteAsync(string key, CancellationToken cancellationToken = default);
}
