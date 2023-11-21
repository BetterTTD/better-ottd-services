using EventBus.Abstractions;
using IntegrationEvents;
using Microsoft.Extensions.Hosting;

namespace OpenTTD.Test;

public sealed class App(IEventBus eventBus) : IHostedService
{
    public Task StartAsync(CancellationToken cancellationToken)
    {
        eventBus.SubscribeAsync<ServerMessageReceivedEvent, ServerMessageReceivedEventHandler>();

        Console.Read();
        
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}

public class ServerMessageReceivedEventHandler : IIntegrationEventHandler<ServerMessageReceivedEvent>
{
    public Task Handle(ServerMessageReceivedEvent @event)
    {
        Console.WriteLine(@event);
        
        return Task.CompletedTask;
    }
}