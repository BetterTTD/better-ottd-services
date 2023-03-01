using MediatR;
using OpenTTD.AdminClient.Services;
using OpenTTD.Domain.Commands;

namespace OpenTTD.AdminClient.Domain.CommandHandlers;

public sealed class RemoveServerCommandHandler : INotificationHandler<RemoveServer>
{
    private readonly ICoordinatorService _coordinator;
    private readonly ILogger<RemoveServerCommandHandler> _logger;

    public RemoveServerCommandHandler(ICoordinatorService coordinator, ILogger<RemoveServerCommandHandler> logger)
    {
        _coordinator = coordinator;
        _logger = logger;
    }

    public async Task Handle(RemoveServer notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "[CMD:{CmdName}] Data {Notification}", 
            nameof(RemoveServerCommandHandler), notification);

        await _coordinator.RemoveServerAsync(notification.Id, cancellationToken);
    }
}