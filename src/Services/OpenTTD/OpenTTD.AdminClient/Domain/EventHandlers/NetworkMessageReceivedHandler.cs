using EventBus.Abstractions;
using IntegrationEvents;
using MediatR;
using Newtonsoft.Json;
using OpenTTD.AdminClientDomain.Events;

namespace OpenTTD.AdminClient.Domain.EventHandlers;

public sealed class NetworkMessageReceivedHandler(ILogger<NetworkMessageReceivedHandler> logger, IEventBus eventBus)
    : INotificationHandler<NetworkMessageReceived>
{
    public Task Handle(NetworkMessageReceived notification, CancellationToken cancellationToken)
    {
        logger.LogInformation(
            "[{Handler}] [ServerId:{ServerId}] Received message: {Message}", 
            nameof(NetworkMessageReceivedHandler), notification.ServerId.Value, notification.Message);

        var @event = new ServerMessageReceivedEvent
        {
            ServerId = notification.ServerId,
            Type = notification.Message.PacketType,
            Message = JsonConvert.SerializeObject(notification.Message)
        };

        eventBus.PublishAsync(@event);
        
        return Task.CompletedTask;
    }
}