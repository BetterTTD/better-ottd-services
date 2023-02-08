using EventBus.Events;

namespace EventBus.Abstractions;

public interface IIntegrationEventHandler<in TEvent> where TEvent : IntegrationEvent
{
    Task Handle(TEvent @event);
}