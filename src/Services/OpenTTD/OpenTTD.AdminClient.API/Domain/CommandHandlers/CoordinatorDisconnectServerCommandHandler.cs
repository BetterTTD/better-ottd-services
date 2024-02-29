using Akka.Util;
using OpenTTD.AdminClient.API.Abstractions;
using OpenTTD.AdminClient.API.Services;
using OpenTTD.AdminClient.Domain.Commands;
using OpenTTD.AdminClient.Domain.ValueObjects;

namespace OpenTTD.AdminClient.API.Domain.CommandHandlers;

public sealed class CoordinatorDisconnectServerCommandHandler(ICoordinatorService coordinator,
        ILogger<CoordinatorDisconnectServerCommandHandler> logger)
    : ICommandHandler<CoordinatorDisconnectServer, ServerId>
{
    public async Task<Result<ServerId>> Handle(CoordinatorDisconnectServer cmd, CancellationToken cancellationToken)
    {
        logger.LogInformation(
            "[CMD:{CmdName}] Data {Notification}", 
            nameof(CoordinatorDisconnectServerCommandHandler), cmd);

        await coordinator.TellToDisconnectServerAsync(cmd.Id, cancellationToken);
        
        return Result.Success(cmd.Id);
    }
}