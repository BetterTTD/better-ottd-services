using EventBus.Abstractions;
using IntegrationEvents;
using OpenTTD.StateService.IntegrationHandlers;

namespace OpenTTD.StateService.HostedService;

public sealed class EventBusHostedService(IEventBus eventBus) : IHostedService
{
    public Task StartAsync(CancellationToken cancellationToken)
    {
        eventBus.SubscribeAsync<ServerMessageReceivedIntegrationEvent, ServerMessageReceivedIntegrationEventHandler>();
        
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}