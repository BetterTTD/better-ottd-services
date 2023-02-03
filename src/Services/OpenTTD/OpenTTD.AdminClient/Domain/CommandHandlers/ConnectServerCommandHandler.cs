using MediatR;
using OpenTTD.AdminClient.Services;
using OpenTTD.Domain.Commands;

namespace OpenTTD.AdminClient.Domain.CommandHandlers;

public sealed class ConnectServerHandler : INotificationHandler<ConnectServer>
{
    private readonly ICoordinatorService _coordinator;

    public ConnectServerHandler(ICoordinatorService coordinator)
    {
        _coordinator = coordinator;
    }

    public async Task Handle(ConnectServer notification, CancellationToken cancellationToken)
    {
        await _coordinator.TellServerToConnectAsync(notification.Id, cancellationToken);
    }
}