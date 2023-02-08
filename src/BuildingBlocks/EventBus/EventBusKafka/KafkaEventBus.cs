using Confluent.Kafka;
using EventBus.Abstractions;
using EventBus.Events;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace EventBusKafka;

public class KafkaEventBus : IEventBus
{
    private readonly IEventBusSubscriptionManager _subscriptionManager;
    private readonly ILogger<KafkaEventBus> _logger;
    private readonly KafkaConnection _kafkaConnection;
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public KafkaEventBus(
        IEventBusSubscriptionManager subscriptionManager, 
        ILogger<KafkaEventBus> logger,
        KafkaConnection kafkaConnection, 
        IServiceProvider serviceProvider)
    {
        _subscriptionManager = subscriptionManager;
        _logger = logger;
        _kafkaConnection = kafkaConnection;
        _serviceScopeFactory = serviceProvider.GetRequiredService<IServiceScopeFactory>();
    }

    public async Task PublishAsync<T>(T @event) where T : IntegrationEvent
    {
        var eventType = typeof(T).Name;

        try
        {
            var producer = _kafkaConnection.ProducerBuilder<T>();
            await producer.ProduceAsync(eventType, new Message<Null, T> { Value = @event });
            producer.Flush();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occured during publishing the event to topic {eventType}", eventType);
        }
    }

    public async Task SubscribeAsync<T, TH>()
        where T : IntegrationEvent
        where TH : IIntegrationEventHandler<T>
    {
        var eventName = typeof(T).Name;
        var consumer = _kafkaConnection.ConsumerBuilder<T>();
        
        _subscriptionManager.AddSubscription<T, TH>();

        consumer.Subscribe(eventName);

        await Task.Run(async () =>
        {
            while (true)
            {
                try
                {
                    var consumerResult = consumer.Consume();
                    await ProcessEvent(consumerResult.Message.Value);
                }
                catch (ConsumeException exn)
                {
                    _logger.LogError(exn, 
                        "Error `{ErrorReason}` occured during consuming the event from topic {eventName}", 
                        exn.Error.Reason, eventName);
                }
            }
        }).ConfigureAwait(false);
    }

    private async Task ProcessEvent<T>(T value) where T : IntegrationEvent
    {
        if (!_subscriptionManager.HasEvent<T>())
        {
            return;
        }

        using var scope = _serviceScopeFactory.CreateScope();
        
        var subscriptions = _subscriptionManager.GetHandlersForEvent<T>();
        foreach (var subscription in subscriptions)
        {
            var handler = scope.ServiceProvider.GetRequiredService(subscription);
            var concreteType = typeof(IIntegrationEventHandler<>).MakeGenericType(typeof(T));

            var task = (Task?)concreteType.GetMethod("Handle")?.Invoke(handler, new object[] { value });
            if (task != null)
            {
                await task;
            }
        }
    }
}