using Akka.Util;
using OpenTTD.AdminClient.API.Abstractions;
using OpenTTD.AdminClient.API.Services;
using OpenTTD.AdminClient.Domain.Commands;
using OpenTTD.AdminClient.Domain.ValueObjects;

namespace OpenTTD.AdminClient.API.CommandHandlers;

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