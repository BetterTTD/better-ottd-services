using EventBus.Abstractions;
using IntegrationEvents;
using OpenTTD.Networking;

namespace OpenTTD.StateService.Handlers;

public sealed class ServerMessageReceivedEventHandler(
    ILogger<ServerMessageReceivedEventHandler> logger, 
    IMessageDeserializer messageDeserializer) 
    : IIntegrationEventHandler<ServerMessageReceivedEvent>
{
    public Task Handle(ServerMessageReceivedEvent @event)
    {
        logger.LogInformation(@event.ToString());

        var deserialized = messageDeserializer.Deserialize(@event.Type, @event.Message);
        
        logger.LogInformation(deserialized.ToString());
        
        return Task.CompletedTask;
    }
}