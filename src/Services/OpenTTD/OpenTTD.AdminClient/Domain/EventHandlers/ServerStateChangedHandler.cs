using MediatR;
using OpenTTD.Domain.Events;

namespace OpenTTD.AdminClient.EventHandlers;

public sealed class ServerStateChangedHandler : INotificationHandler<ServerStateChanged>
{
    private readonly ILogger<ServerStateChangedHandler> _logger;

    public ServerStateChangedHandler(ILogger<ServerStateChangedHandler> logger)
    {
        _logger = logger;
    }

    public Task Handle(ServerStateChanged notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "[ServerId:{ServerId}] State changed from: '{From}' to: '{To}'", 
            notification.Id.Value, notification.FromState.ToString(), notification.ToState.ToString());
        
        return Task.CompletedTask;
    }
}