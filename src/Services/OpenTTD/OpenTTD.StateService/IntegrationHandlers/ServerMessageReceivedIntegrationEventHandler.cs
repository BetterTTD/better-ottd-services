using EventBus.Abstractions;
using IntegrationEvents;
using OpenTTD.AdminClient.Networking;

namespace OpenTTD.StateService.IntegrationHandlers;

public sealed class ServerMessageReceivedIntegrationEventHandler(
    ILogger<ServerMessageReceivedIntegrationEventHandler> logger, 
    IMessageDeserializer messageDeserializer) 
    : IIntegrationEventHandler<ServerMessageReceivedIntegrationEvent>
{
    public Task Handle(ServerMessageReceivedIntegrationEvent integrationEvent)
    {
        logger.LogInformation(integrationEvent.ToString());

        var deserialized = messageDeserializer.Deserialize(integrationEvent.Type, integrationEvent.Message);
        
        logger.LogInformation(deserialized.ToString());
        
        return Task.CompletedTask;
    }
}