using MediatR;
using OpenTTD.AdminClientDomain.Events;

namespace OpenTTD.AdminClient.Domain.EventHandlers;

public sealed class NetworkMessageSentHandler(ILogger<NetworkMessageSentHandler> logger) : INotificationHandler<NetworkMessageSent>
{
    public Task Handle(NetworkMessageSent notification, CancellationToken cancellationToken)
    {
        logger.LogInformation(
            "[{Handler}] [ServerId:{ServerId}] Sent message: {Message}", 
            nameof(NetworkMessageSentHandler), notification.ServerId.Value, notification.Message);
        
        return Task.CompletedTask;
    }
}