using MediatR;
using OpenTTD.AdminClientDomain.Events;

namespace OpenTTD.AdminClient.Domain.EventHandlers;

public sealed class ServerStateChangedHandler(ILogger<ServerStateChangedHandler> logger) : INotificationHandler<ServerStateChanged>
{
    public Task Handle(ServerStateChanged notification, CancellationToken cancellationToken)
    {
        logger.LogInformation(
            "[{Handler}] [ServerId:{ServerId}] State changed from: '{From}' to: '{To}'", 
            nameof(ServerStateChangedHandler), notification.ServerId.Value, notification.FromState, notification.ToState);
        
        return Task.CompletedTask;
    }
}