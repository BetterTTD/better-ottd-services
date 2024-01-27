using MediatR;
using Newtonsoft.Json;
using OpenTTD.AdminClient.Domain.Events;

namespace OpenTTD.AdminClient.Domain.EventHandlers;

public sealed class NetworkMessageReceivedHandler(ILogger<NetworkMessageReceivedHandler> logger)
    : INotificationHandler<NetworkMessageReceived>
{
    public Task Handle(NetworkMessageReceived notification, CancellationToken cancellationToken)
    {
        logger.LogInformation(
            "[{Handler}] [ServerId:{ServerId}] Received message: {Message}", 
            nameof(NetworkMessageReceivedHandler), notification.ServerId.Value, notification.Message);

        var @event = new
        {
            ServerId = notification.ServerId,
            Type = notification.Message.PacketType,
            Message = JsonConvert.SerializeObject(notification.Message)
        };

        return Task.CompletedTask;
    }
}