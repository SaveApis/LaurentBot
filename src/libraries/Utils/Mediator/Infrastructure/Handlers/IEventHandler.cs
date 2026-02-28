using MediatR;
using Utils.Mediator.Infrastructure.Events;

namespace Utils.Mediator.Infrastructure.Handlers;

public interface IEventHandler<in TEvent> : INotificationHandler<TEvent> where TEvent : IEvent;
