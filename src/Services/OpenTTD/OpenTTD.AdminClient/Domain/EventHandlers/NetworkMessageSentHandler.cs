using MediatR;
using OpenTTD.Domain.Events;

namespace OpenTTD.AdminClient.EventHandlers;

public sealed class NetworkMessageSentHandler : INotificationHandler<NetworkMessageSent>
{
    private readonly ILogger<NetworkMessageSentHandler> _logger;

    public NetworkMessageSentHandler(ILogger<NetworkMessageSentHandler> logger)
    {
        _logger = logger;
    }

    public Task Handle(NetworkMessageSent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "[ServerId:{ServerId}] Sent message: {Message}", 
            notification.Id.Value, notification.Message.ToString());
        
        return Task.CompletedTask;
    }
}