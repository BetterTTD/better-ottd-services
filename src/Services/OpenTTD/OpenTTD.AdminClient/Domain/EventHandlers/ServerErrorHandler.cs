using MediatR;
using OpenTTD.AdminClient.Domain.Events;

namespace OpenTTD.AdminClient.Domain.EventHandlers;

public sealed class ServerErrorHandler(ILogger<ServerErrorHandler> logger) : INotificationHandler<ServerError>
{
    public Task Handle(ServerError notification, CancellationToken cancellationToken)
    {
        logger.LogError(notification.Exception,
            "[{Handler}] [ServerId:{ServerId}] Received an error: {Error}", 
            nameof(ServerErrorHandler), notification.ServerId.Value, notification.Message);

        return Task.CompletedTask;
    }
}