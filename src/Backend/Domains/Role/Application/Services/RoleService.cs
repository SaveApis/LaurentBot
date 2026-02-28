using Backend.Domains.Role.Application.Exceptions;
using Backend.Domains.Role.Application.Mapping;
using Backend.Domains.Role.Application.Models.DTO;
using Backend.Domains.Role.Domain.Models.Entities;
using Backend.Persistence.Sql.Context;
using Microsoft.EntityFrameworkCore;

namespace Backend.Domains.Role.Application.Services;

public class RoleService(ILogger<IRoleService> logger, IDbContextFactory<CoreDbContext> factory, IRoleMapper mapper) : IRoleService
{
    public async Task<IReadOnlyList<RoleGetDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        logger.LogInformation("Start getting all roles");

        logger.LogTrace("Instantiate database context");
        var context = await factory.CreateDbContextAsync(cancellationToken).ConfigureAwait(false);
        await using var _ = context.ConfigureAwait(false);

        logger.LogDebug("Querying all roles from database");
        var entities = await context.Roles.ToListAsync(cancellationToken).ConfigureAwait(false);

        logger.LogTrace("Mapping entities to DTOs");
        var dtos = mapper.ToDto(entities).ToList();

        logger.LogInformation("Finished getting '{Count}' roles", dtos.Count);

        return dtos.AsReadOnly();
    }

    public async Task<RoleGetDto> GetByKeyAsync(string key, CancellationToken cancellationToken = default)
    {
        logger.LogInformation("Start getting role with key '{Key}'", key);

        logger.LogTrace("Instantiate database context");
        var context = await factory.CreateDbContextAsync(cancellationToken).ConfigureAwait(false);
        await using var _ = context.ConfigureAwait(false);

        logger.LogDebug("Querying role with key '{Key}'", key);
        var entity = await context.Roles.FirstOrDefaultAsync(r => r.Key == key, cancellationToken).ConfigureAwait(false);
        if (entity == null)
        {
            logger.LogWarning("Role with key '{Key}' not found", key);
            throw new RoleNotFoundException(key);
        }

        logger.LogTrace("Mapping entity to DTO");
        var dto = mapper.ToDto(entity);

        logger.LogInformation("Finished getting role with key '{Key}'", key);
        return dto;
    }

    public async Task<RoleGetDto> CreateAsync(RoleCreateDto dto, CancellationToken cancellationToken = default)
    {
        logger.LogInformation("Start creating new role");

        logger.LogTrace("Instantiate database context");
        var context = await factory.CreateDbContextAsync(cancellationToken).ConfigureAwait(false);
        await using var _ = context.ConfigureAwait(false);

        logger.LogTrace("Querying database for existing role with key '{Key}'", dto.Key);
        var existing = await context.Roles.SingleOrDefaultAsync(r => r.Key == dto.Key, cancellationToken).ConfigureAwait(false);
        if (existing != null)
        {
            logger.LogWarning("Role with key '{Key}' already exists", dto.Key);
            throw new RoleAlreadyExistsException(existing.Key);
        }

        logger.LogTrace("Creating new role entity");
        var entity = RoleEntity.Create(dto.Key, dto.Name, dto.Description);
        await context.Roles.AddAsync(entity, cancellationToken).ConfigureAwait(false);

        logger.LogDebug("Saving new role to database");
        await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

        logger.LogTrace("Mapping entity to DTO");
        var resultDto = mapper.ToDto(entity);

        logger.LogInformation("Finished creating new role with key '{Key}'", entity.Key);
        return resultDto;
    }

    public async Task<RoleGetDto> UpdateAsync(string key, RoleUpdateDto dto, CancellationToken cancellationToken = default)
    {
        logger.LogInformation("Start updating role with key '{Key}'", key);

        logger.LogTrace("Instantiate database context");
        var context = await factory.CreateDbContextAsync(cancellationToken).ConfigureAwait(false);
        await using var _ = context.ConfigureAwait(false);

        logger.LogDebug("Querying role with key '{Key}'", key);
        var entity = await context.Roles.SingleOrDefaultAsync(r => r.Key == key, cancellationToken).ConfigureAwait(false);
        if (entity == null)
        {
            logger.LogWarning("Role with key '{Key}' not found", key);
            throw new RoleNotFoundException(key);
        }

        logger.LogTrace("Updating role entity");
        entity.Update(dto.Name, dto.Description);

        logger.LogDebug("Saving updated role to database");
        await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

        logger.LogTrace("Mapping entity to DTO");
        var resultDto = mapper.ToDto(entity);

        logger.LogInformation("Finished updating role with key '{Key}'", key);
        return resultDto;
    }

    public async Task DeleteAsync(string key, CancellationToken cancellationToken = default)
    {
        logger.LogInformation("Start deleting role with key '{Key}'", key);

        logger.LogTrace("Instantiate database context");
        var context = await factory.CreateDbContextAsync(cancellationToken).ConfigureAwait(false);
        await using var _ = context.ConfigureAwait(false);

        logger.LogDebug("Querying role with key '{Key}'", key);
        var entity = await context.Roles.SingleOrDefaultAsync(r => r.Key == key, cancellationToken).ConfigureAwait(false);
        if (entity == null)
        {
            logger.LogWarning("Role with key '{Key}' not found", key);
            throw new RoleNotFoundException(key);
        }

        logger.LogTrace("Removing role entity from database");
        context.Roles.Remove(entity);

        logger.LogDebug("Saving changes to database");
        await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

        logger.LogInformation("Finished deleting role with key '{Key}'", key);
    }
}
