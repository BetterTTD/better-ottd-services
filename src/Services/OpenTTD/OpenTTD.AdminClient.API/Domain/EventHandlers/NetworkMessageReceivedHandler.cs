using IntegrationEvents;
using MassTransit;
using MediatR;
using Newtonsoft.Json;
using OpenTTD.AdminClient.Domain.Events;

namespace OpenTTD.AdminClient.API.Domain.EventHandlers;

public sealed class NetworkMessageReceivedHandler(
    ILogger<NetworkMessageReceivedHandler> logger,
    IBus bus)
    : INotificationHandler<NetworkMessageReceived>
{
    public async Task Handle(NetworkMessageReceived notification, CancellationToken cancellationToken)
    {
        logger.LogInformation(
            "[{Handler}] [ServerId:{ServerId}] Received message: {Message}", 
            nameof(NetworkMessageReceivedHandler), notification.ServerId.Value, notification.Message);

        var @event = new ServerNetworkMessageReceived
        {
            ServerId = notification.ServerId.Value,
            PacketType = notification.Message.PacketType,
            Message = JsonConvert.SerializeObject(notification.Message)
        };

        await bus.Publish(@event, cancellationToken);
    }
}