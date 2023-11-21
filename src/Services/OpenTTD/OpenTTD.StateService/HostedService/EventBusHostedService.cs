using EventBus.Abstractions;

namespace OpenTTD.StateService;

public sealed class EventBusHostedService : IHostedService
{
    private readonly IEventBus _eventBus;

    public EventBusHostedService(IEventBus eventBus)
    {
        _eventBus = eventBus;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}