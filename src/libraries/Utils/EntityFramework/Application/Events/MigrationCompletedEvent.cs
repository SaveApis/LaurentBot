using Utils.Mediator.Infrastructure.Events;

namespace Utils.EntityFramework.Application.Events;

public class MigrationCompletedEvent(Type contextType) : IEvent
{
    public Type ContextType { get; } = contextType;
}
