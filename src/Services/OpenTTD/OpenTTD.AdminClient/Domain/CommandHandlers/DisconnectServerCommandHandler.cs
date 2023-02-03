using MediatR;
using OpenTTD.AdminClient.Services;
using OpenTTD.Domain.Commands;

namespace OpenTTD.AdminClient.Domain.CommandHandlers;

public sealed class DisconnectServerCommandHandler : INotificationHandler<DisconnectServer>
{
    private readonly ICoordinatorService _coordinator;

    public DisconnectServerCommandHandler(ICoordinatorService coordinator)
    {
        _coordinator = coordinator;
    }

    public async Task Handle(DisconnectServer notification, CancellationToken cancellationToken)
    {
        await _coordinator.TellServerToDisconnectAsync(notification.Id, cancellationToken);
    }
}