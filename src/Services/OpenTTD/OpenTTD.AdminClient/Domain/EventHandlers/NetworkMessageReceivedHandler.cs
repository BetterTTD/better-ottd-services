using MediatR;
using OpenTTD.Domain.Events;

namespace OpenTTD.AdminClient.Domain.EventHandlers;

public sealed class NetworkMessageReceivedHandler : INotificationHandler<NetworkMessageReceived>
{
    private readonly ILogger<NetworkMessageReceivedHandler> _logger;

    public NetworkMessageReceivedHandler(ILogger<NetworkMessageReceivedHandler> logger)
    {
        _logger = logger;
    }

    public Task Handle(NetworkMessageReceived notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "[ServerId:{ServerId}] Received message: {Message}", 
            notification.ServerId.Value, notification.Message);
        
        return Task.CompletedTask;
    }
}