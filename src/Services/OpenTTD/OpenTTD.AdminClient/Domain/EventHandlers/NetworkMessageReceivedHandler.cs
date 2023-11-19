using MediatR;
using OpenTTD.AdminClientDomain.Events;

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
            "[{Handler}] [ServerId:{ServerId}] Received message: {Message}", 
            nameof(NetworkMessageReceivedHandler), notification.ServerId.Value, notification.Message);
        
        return Task.CompletedTask;
    }
}