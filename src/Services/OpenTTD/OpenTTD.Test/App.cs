using EventBus.Abstractions;
using IntegrationEvents;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using OpenTTD.Networking.Messages;

namespace OpenTTD.Test;

public sealed class App : IHostedService
{
    private readonly IEventBus _eventBus;

    public App(IEventBus eventBus)
    {
        _eventBus = eventBus;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _eventBus.SubscribeAsync<ServerMessageReceivedEvent, ServerMessageReceivedEventHandler>();

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