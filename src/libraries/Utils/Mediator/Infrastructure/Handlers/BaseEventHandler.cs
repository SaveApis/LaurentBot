using Microsoft.Extensions.Logging;
using Utils.Mediator.Infrastructure.Events;

namespace Utils.Mediator.Infrastructure.Handlers;

public abstract class BaseEventHandler<THandler, TEvent>(ILogger<THandler> logger) : IEventHandler<TEvent> where TEvent : IEvent
{
    protected ILogger<THandler> Logger { get; } = logger;

    public async Task Handle(TEvent notification, CancellationToken cancellationToken)
    {
        Logger.LogInformation("Received event '{Event}' for handler '{Handler}'", typeof(TEvent).Name, typeof(THandler).Name);

        Logger.LogTrace("Checking if event '{Event}' is supported by handler '{Handler}'", typeof(TEvent).Name, typeof(THandler).Name);
        if (!await SupportAsync(notification, cancellationToken).ConfigureAwait(false))
        {
            Logger.LogInformation("Event '{Event}' is not supported by handler '{Handler}'. Skipping.", typeof(TEvent).Name, typeof(THandler).Name);
            return;
        }

        Logger.LogTrace("Handling event '{Event}' with handler '{Handler}'", typeof(TEvent).Name, typeof(THandler).Name);
        await HandleAsync(notification, cancellationToken).ConfigureAwait(false);
    }

    protected virtual Task<bool> SupportAsync(TEvent notification, CancellationToken cancellationToken)
    {
        return Task.FromResult(Support(notification));
    }
    protected virtual bool Support(TEvent notification)
    {
        return true;
    }

    protected abstract Task HandleAsync(TEvent notification, CancellationToken cancellationToken);
}
