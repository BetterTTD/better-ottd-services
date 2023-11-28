using Akka.Util;
using OpenTTD.AdminClient.Domain.Abstractions;
using OpenTTD.AdminClient.Services;
using OpenTTD.AdminClientDomain.Commands;
using OpenTTD.AdminClientDomain.ValueObjects;

namespace OpenTTD.AdminClient.Domain.CommandHandlers;

public sealed class DisconnectServerCommandHandler(ICoordinatorService coordinator,
        ILogger<DisconnectServerCommandHandler> logger)
    : ICommandHandler<DisconnectServer, ServerId>
{
    public async Task<Result<ServerId>> Handle(DisconnectServer cmd, CancellationToken cancellationToken)
    {
        logger.LogInformation(
            "[CMD:{CmdName}] Data {Notification}", 
            nameof(DisconnectServerCommandHandler), cmd);

        await coordinator.TellServerToDisconnectAsync(cmd.Id, cancellationToken);
        
        return Result.Success(cmd.Id);
    }
}