using IntegrationEvents;
using MassTransit;

namespace OpenTTD.StateService.IntegrationConsumers;

public class ServerNetworkMessageReceivedConsumer(ILogger<ServerNetworkMessageReceivedConsumer> logger)
    : IConsumer<ServerNetworkMessageReceived>
{
    public Task Consume(ConsumeContext<ServerNetworkMessageReceived> context)
    {
        var @event = context.Message;
        
        logger.LogInformation(@event.Message);
        
        return Task.CompletedTask;
    }
}