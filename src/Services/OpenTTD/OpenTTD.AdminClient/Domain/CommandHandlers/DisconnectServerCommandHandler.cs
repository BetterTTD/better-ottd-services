using MediatR;
using OpenTTD.AdminClient.Services;
using OpenTTD.Domain.Commands;

namespace OpenTTD.AdminClient.Domain.CommandHandlers;

public sealed class DisconnectServerCommandHandler : INotificationHandler<DisconnectServer>
{
    private readonly ICoordinatorService _coordinator;
    private readonly ILogger<DisconnectServerCommandHandler> _logger;

    public DisconnectServerCommandHandler(
        ICoordinatorService coordinator,
        ILogger<DisconnectServerCommandHandler> logger)
    {
        _coordinator = coordinator;
        _logger = logger;
    }

    public async Task Handle(DisconnectServer notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "[CMD:{CmdName}] Data {Notification}", 
            nameof(DisconnectServerCommandHandler), notification);

        await _coordinator.TellServerToDisconnectAsync(notification.Id, cancellationToken);
    }
}