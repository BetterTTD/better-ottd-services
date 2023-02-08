using EventBus.Events;

namespace EventBus.Abstractions;

public interface IEventBus
{
    Task PublishAsync<T>(T @event) where T : IntegrationEvent;
    Task SubscribeAsync<T, TH>()
        where T : IntegrationEvent
        where TH : IIntegrationEventHandler<T>;
}