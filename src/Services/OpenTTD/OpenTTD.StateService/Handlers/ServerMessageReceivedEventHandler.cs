using EventBus.Abstractions;
using IntegrationEvents;

namespace OpenTTD.StateService.Handlers;

public sealed class ServerMessageReceivedEventHandler : IIntegrationEventHandler<ServerMessageReceivedEvent>
{
    private readonly ILogger<ServerMessageReceivedEventHandler> _logger;

    public ServerMessageReceivedEventHandler(ILogger<ServerMessageReceivedEventHandler> logger)
    {
        _logger = logger;
    }

    public Task Handle(ServerMessageReceivedEvent @event)
    {
        _logger.LogInformation(@event.ToString());
        
        return Task.CompletedTask;
    }
}