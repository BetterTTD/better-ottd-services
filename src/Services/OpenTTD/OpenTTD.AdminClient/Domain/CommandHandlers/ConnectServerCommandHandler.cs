using MediatR;
using OpenTTD.AdminClient.Services;
using OpenTTD.Domain.Commands;

namespace OpenTTD.AdminClient.Domain.CommandHandlers;

public sealed class ConnectServerHandler : INotificationHandler<ConnectServer>
{
    private readonly ICoordinatorService _coordinator;
    private readonly ILogger<ConnectServerHandler> _logger;

    public ConnectServerHandler(ICoordinatorService coordinator, ILogger<ConnectServerHandler> logger)
    {
        _coordinator = coordinator;
        _logger = logger;
    }

    public async Task Handle(ConnectServer notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "[CMD:{CmdName}] Data {Notification}", 
            nameof(ConnectServerHandler), notification);

        await _coordinator.TellServerToConnectAsync(notification.Id, cancellationToken);
    }
}