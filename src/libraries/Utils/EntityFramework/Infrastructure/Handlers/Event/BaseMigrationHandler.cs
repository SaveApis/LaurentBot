using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Utils.EntityFramework.Application.Events;
using Utils.EntityFramework.Infrastructure.Persistence.Sql.Context;
using Utils.Mediator.Application.Events;
using Utils.Mediator.Infrastructure.Handlers;

namespace Utils.EntityFramework.Infrastructure.Handlers.Event;

public abstract class BaseMigrationHandler<THandler, TContext>(ILogger<THandler> logger, IDbContextFactory<TContext> factory, IMediator mediator)
    : IEventHandler<ApplicationStartedEvent> where TContext : BaseDbContext<TContext> where THandler : BaseMigrationHandler<THandler, TContext>
{
    public async Task Handle(ApplicationStartedEvent notification, CancellationToken cancellationToken)
    {
        logger.LogInformation("Starting database migration for '{ContextName}'", typeof(TContext).Name);

        logger.LogTrace("Creating database context instance for migration");
        var context = await factory.CreateDbContextAsync(cancellationToken);
        await using var _ = context.ConfigureAwait(false);

        logger.LogDebug("Applying pending migrations to the database");
        await context.Database.MigrateAsync(cancellationToken).ConfigureAwait(false);

        logger.LogInformation("Database migration completed for '{ContextName}'", typeof(TContext).Name);
        await mediator.Publish(new MigrationCompletedEvent(context.GetType()), cancellationToken).ConfigureAwait(false);
    }
}
