using Akka.Util;
using OpenTTD.AdminClient.Domain.Abstractions;
using OpenTTD.AdminClient.Services;
using OpenTTD.AdminClientDomain.Commands;
using OpenTTD.AdminClientDomain.ValueObjects;

namespace OpenTTD.AdminClient.Domain.CommandHandlers;

public sealed class DisconnectServerCommandHandler : ICommandHandler<DisconnectServer, ServerId>
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

    public async Task<Result<ServerId>> Handle(DisconnectServer cmd, CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "[CMD:{CmdName}] Data {Notification}", 
            nameof(DisconnectServerCommandHandler), cmd);

        await _coordinator.TellServerToDisconnectAsync(cmd.Id, cancellationToken);
        
        return Result.Success(cmd.Id);
    }
}