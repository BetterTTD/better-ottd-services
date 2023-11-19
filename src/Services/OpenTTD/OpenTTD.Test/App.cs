using EventBus.Abstractions;
using IntegrationEvents;
using Microsoft.Extensions.Hosting;

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
        _eventBus.SubscribeAsync<TestEvent, TestEventHandler>();

        Console.Read();
        
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}

public class TestEventHandler : IIntegrationEventHandler<TestEvent>
{
    public Task Handle(TestEvent @event)
    {
        Console.WriteLine(@event);
        
        return Task.CompletedTask;
    }
}