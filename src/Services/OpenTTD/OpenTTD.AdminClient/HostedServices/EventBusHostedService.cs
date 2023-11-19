using EventBus.Abstractions;

namespace OpenTTD.AdminClient.HostedServices;

public class EventBusHostedService : IHostedService
{
    private readonly IEventBusSubscriptionManager _subscriptionManager;

    public EventBusHostedService(IEventBusSubscriptionManager subscriptionManager)
    {
        _subscriptionManager = subscriptionManager;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}