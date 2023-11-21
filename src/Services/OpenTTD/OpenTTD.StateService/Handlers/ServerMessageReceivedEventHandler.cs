using EventBus.Abstractions;
using IntegrationEvents;

namespace OpenTTD.StateService.Handlers;

public sealed class ServerMessageReceivedEventHandler(ILogger<ServerMessageReceivedEventHandler> logger) 
    : IIntegrationEventHandler<ServerMessageReceivedEvent>
{
    public Task Handle(ServerMessageReceivedEvent @event)
    {
        logger.LogInformation(@event.ToString());
        
        return Task.CompletedTask;
    }
}