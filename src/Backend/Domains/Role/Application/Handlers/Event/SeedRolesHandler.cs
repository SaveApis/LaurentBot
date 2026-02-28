using Backend.Domains.Role.Application.Mapping;
using Backend.Domains.Role.Application.Services;
using Backend.Domains.Role.Infrastructure.Roles;
using Utils.EntityFramework.Application.Events;
using Utils.Mediator.Infrastructure.Handlers;

namespace Backend.Domains.Role.Application.Handlers.Event;

public class SeedRolesHandler(ILogger<SeedRolesHandler> logger, IRoleService service, IRoleMapper mapper, IEnumerable<IRole> roles) : BaseEventHandler<SeedRolesHandler, MigrationCompletedEvent>(logger)
{
    protected override async Task HandleAsync(MigrationCompletedEvent notification, CancellationToken cancellationToken)
    {
        Logger.LogInformation("Start seeding roles...");

        Logger.LogDebug("Getting existing roles from database and code...");
        var dbRoles = await service.GetAllAsync(cancellationToken);
        var codeRoles = roles.ToList();

        Logger.LogTrace("Extract keys and compare...");
        var dbKeys = dbRoles.Select(x => x.Key).ToList();
        var codeKeys = codeRoles.Select(x => x.Key).ToList();

        var toAdd = codeKeys.Except(dbKeys).ToList();
        var toRemove = dbKeys.Except(codeKeys).ToList();
        var toUpdate = codeKeys.Intersect(dbKeys).ToList();
        
        Logger.LogInformation("Roles to add: {AddCount}, to remove: {RemoveCount}, to update: {UpdateCount}", toAdd.Count, toRemove.Count, toUpdate.Count);
        
        var addTask = AddAsync(codeRoles, toAdd, cancellationToken);
        var removeTask = RemoveAsync(toRemove, cancellationToken);
        var updateTask = UpdateAsync(codeRoles, toUpdate, cancellationToken);

        await Task.WhenAll(addTask, removeTask, updateTask);

        Logger.LogInformation("Roles seeding completed. Added: {AddCount}, Removed: {RemoveCount}, Updated: {UpdateCount}", toAdd.Count, toRemove.Count, toUpdate.Count);
    }

    private async Task AddAsync(List<IRole> roles, List<string> toAdd, CancellationToken cancellationToken)
    {
        var filteredRoles = roles.Where(x => toAdd.Contains(x.Key)).ToList();

        foreach (var dto in filteredRoles.Select(mapper.ToCreateDto))
        {
            await service.CreateAsync(dto, cancellationToken).ConfigureAwait(false);
        }
    }

    private async Task RemoveAsync(List<string> keys, CancellationToken cancellationToken)
    {
        foreach (var key in keys)
        {
            await service.DeleteAsync(key, cancellationToken).ConfigureAwait(false);
        }
    }

    private async Task UpdateAsync(List<IRole> codeRoles, List<string> toUpdate, CancellationToken cancellationToken)
    {
        var dictionary = codeRoles.Where(x => toUpdate.Contains(x.Key)).ToDictionary(x => x.Key, x => x);

        foreach (var (key, role) in dictionary)
        {
            var dto = mapper.ToUpdateDto(role);
            await service.UpdateAsync(key, dto, cancellationToken).ConfigureAwait(false);
        }
    }
}
