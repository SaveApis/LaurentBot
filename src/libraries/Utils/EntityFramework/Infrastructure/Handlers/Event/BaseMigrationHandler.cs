using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Utils.EntityFramework.Application.Events;
using Utils.EntityFramework.Infrastructure.Persistence.Sql.Context;
using Utils.Mediator.Application.Events;
using Utils.Mediator.Infrastructure.Handlers;

namespace Utils.EntityFramework.Infrastructure.Handlers.Event;

public abstract class BaseMigrationHandler<THandler, TContext>(ILogger<THandler> logger, IDbContextFactory<TContext> factory, IMediator mediator)
    : BaseEventHandler<THandler, ApplicationStartedEvent>(logger) where TContext : BaseDbContext<TContext> where THandler : BaseMigrationHandler<THandler, TContext>
{
    protected override async Task HandleAsync(ApplicationStartedEvent notification, CancellationToken cancellationToken)
    {
        Logger.LogInformation("Starting database migration for '{ContextName}'", typeof(TContext).Name);

        Logger.LogTrace("Creating database context instance for migration");
        var context = await factory.CreateDbContextAsync(cancellationToken);
        await using var _ = context.ConfigureAwait(false);

        Logger.LogDebug("Applying pending migrations to the database");
        await context.Database.MigrateAsync(cancellationToken).ConfigureAwait(false);

        Logger.LogInformation("Database migration completed for '{ContextName}'", typeof(TContext).Name);
        await mediator.Publish(new MigrationCompletedEvent(context.GetType()), cancellationToken).ConfigureAwait(false);
    }
}
