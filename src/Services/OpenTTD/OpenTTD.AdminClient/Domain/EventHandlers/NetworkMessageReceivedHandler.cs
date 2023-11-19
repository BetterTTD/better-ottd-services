using EventBus.Abstractions;
using IntegrationEvents;
using MediatR;
using Newtonsoft.Json;
using OpenTTD.AdminClientDomain.Events;

namespace OpenTTD.AdminClient.Domain.EventHandlers;

public sealed class NetworkMessageReceivedHandler : INotificationHandler<NetworkMessageReceived>
{
    private readonly ILogger<NetworkMessageReceivedHandler> _logger;
    private readonly IEventBus _eventBus;

    public NetworkMessageReceivedHandler(ILogger<NetworkMessageReceivedHandler> logger, IEventBus eventBus)
    {
        _logger = logger;
        _eventBus = eventBus;
    }

    public Task Handle(NetworkMessageReceived notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "[{Handler}] [ServerId:{ServerId}] Received message: {Message}", 
            nameof(NetworkMessageReceivedHandler), notification.ServerId.Value, notification.Message);

        var @event = new TestEvent
        {
            ServerId = notification.ServerId,
            Type = notification.Message.PacketType,
            Message = JsonConvert.SerializeObject(notification.Message)
        };

        _eventBus.PublishAsync(@event);
        
        return Task.CompletedTask;
    }
}