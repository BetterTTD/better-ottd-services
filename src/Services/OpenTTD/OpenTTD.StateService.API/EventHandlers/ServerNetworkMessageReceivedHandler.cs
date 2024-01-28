using IntegrationEvents;
using MassTransit;

namespace OpenTTD.StateService.API.EventHandlers;

public class ServerNetworkMessageReceivedHandler(ILogger<ServerNetworkMessageReceivedHandler> logger)
    : IConsumer<ServerNetworkMessageReceived>
{
    public Task Consume(ConsumeContext<ServerNetworkMessageReceived> context)
    {
        var @event = context.Message;
        
        logger.LogInformation(@event.Message);
        
        return Task.CompletedTask;
    }
}