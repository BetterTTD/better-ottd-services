using EventBus.Abstractions;
using IntegrationEvents;
using Microsoft.Extensions.Hosting;

namespace OpenTTD.ConsoleTest;

public sealed class App(IEventBus eventBus) : IHostedService
{
    public Task StartAsync(CancellationToken cancellationToken)
    {
        eventBus.SubscribeAsync<ServerMessageReceivedIntegrationEvent, ServerMessageReceivedEventHandler>();

        Console.Read();
        
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}

public class ServerMessageReceivedEventHandler : IIntegrationEventHandler<ServerMessageReceivedIntegrationEvent>
{
    public Task Handle(ServerMessageReceivedIntegrationEvent integrationEvent)
    {
        Console.WriteLine(integrationEvent);
        
        return Task.CompletedTask;
    }
}