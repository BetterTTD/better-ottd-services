using MediatR;
using OpenTTD.Domain.Events;

namespace OpenTTD.AdminClient.Domain.EventHandlers;

public sealed class ServerErrorHandler : INotificationHandler<ServerError>
{
    private readonly ILogger<ServerErrorHandler> _logger;

    public ServerErrorHandler(ILogger<ServerErrorHandler> logger)
    {
        _logger = logger;
    }

    public Task Handle(ServerError notification, CancellationToken cancellationToken)
    {
        _logger.LogError(notification.Exception,
            "[{Handler}] [ServerId:{ServerId}] Received an error: {Error}", 
            nameof(ServerErrorHandler), notification.ServerId.Value, notification.Message);

        return Task.CompletedTask;
    }
}