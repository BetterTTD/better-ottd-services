using MediatR;
using OpenTTD.AdminClientDomain.Events;

namespace OpenTTD.AdminClient.Domain.EventHandlers;

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
            "[{Handler}] [ServerId:{ServerId}] Sent message: {Message}", 
            nameof(NetworkMessageSentHandler), notification.ServerId.Value, notification.Message);
        
        return Task.CompletedTask;
    }
}