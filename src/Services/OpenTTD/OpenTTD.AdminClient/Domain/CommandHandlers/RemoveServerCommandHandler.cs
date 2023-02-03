using MediatR;
using OpenTTD.AdminClient.Services;
using OpenTTD.Domain.Commands;

namespace OpenTTD.AdminClient.Domain.CommandHandlers;

public sealed class RemoveServerCommandHandler : INotificationHandler<RemoveServer>
{
    private readonly ICoordinatorService _coordinator;

    public RemoveServerCommandHandler(ICoordinatorService coordinator)
    {
        _coordinator = coordinator;
    }

    public async Task Handle(RemoveServer notification, CancellationToken cancellationToken)
    {
        await _coordinator.RemoveServerAsync(notification.Id, cancellationToken);
    }
}