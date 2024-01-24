using EventBus.Abstractions;
using EventBus.Events;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace EventBusRedis;

public sealed class RedisEventBus(IEventBusSubscriptionManager subscriptionManager,
        ILogger<RedisEventBus> logger,
        RedisConnection redisConnection,
        IServiceProvider serviceProvider)
    : IEventBus
{
    private readonly IServiceScopeFactory _serviceScopeFactory = ServiceProviderServiceExtensions.GetRequiredService<IServiceScopeFactory>(serviceProvider);


    public async Task PublishAsync<T>(T @event) where T : IntegrationEvent
    {
        var eventType = typeof(T).Name;

        try
        {
            var producer = redisConnection.ProducerBuilder();
            var json = JsonConvert.SerializeObject(@event);
            await producer.PublishAsync(eventType, json);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error occured during publishing the event to channel {eventType}", eventType);
        }
    }

    public async Task SubscribeAsync<T, TH>()
        where T : IntegrationEvent
        where TH : IIntegrationEventHandler<T>
    {
        var eventName = typeof(T).Name;
        var consumer = redisConnection.ConsumerBuilder();
        
        subscriptionManager.AddSubscription<T, TH>();

        await consumer.SubscribeAsync(eventName, (_, redisEvent) =>
        {
            Task.Run(async () =>
            {
                try
                {
                    var @event = JsonConvert.DeserializeObject<T>(redisEvent);
                    await ProcessEvent(@event);
                }
                catch (Exception exn)
                {
                    logger.LogError(exn,
                        "Error `{ErrorReason}` occured during consuming the event from topic {eventName}",
                        exn.Message, eventName);
                }
            }).ConfigureAwait(false);

        }, CommandFlags.FireAndForget);
    }
    
    private async Task ProcessEvent<T>(T value) where T : IntegrationEvent
    {
        if (!subscriptionManager.HasEvent<T>())
        {
            return;
        }

        using var scope = _serviceScopeFactory.CreateScope();
        
        var subscriptions = subscriptionManager.GetHandlersForEvent<T>();
        foreach (var subscription in subscriptions)
        {
            var handler = scope.ServiceProvider.GetRequiredService(subscription);
            var concreteType = typeof(IIntegrationEventHandler<>).MakeGenericType(typeof(T));

            var task = (Task?)concreteType.GetMethod("Handle")?.Invoke(handler, [value]);
            if (task != null)
            {
                await task;
            }
        }
    }
}