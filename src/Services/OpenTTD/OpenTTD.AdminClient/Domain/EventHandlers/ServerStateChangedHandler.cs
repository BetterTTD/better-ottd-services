﻿using MediatR;
using OpenTTD.Domain.Events;

namespace OpenTTD.AdminClient.Domain.EventHandlers;

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
            notification.ServerId.Value, notification.FromState, notification.ToState);
        
        return Task.CompletedTask;
    }
}