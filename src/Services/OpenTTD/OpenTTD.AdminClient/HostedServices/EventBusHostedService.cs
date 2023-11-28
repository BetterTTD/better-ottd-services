using EventBus.Abstractions;

namespace OpenTTD.AdminClient.HostedServices;

public class EventBusHostedService(IEventBusSubscriptionManager subscriptionManager) : IHostedService
{
    private readonly IEventBusSubscriptionManager _subscriptionManager = subscriptionManager;

    public Task StartAsync(CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}