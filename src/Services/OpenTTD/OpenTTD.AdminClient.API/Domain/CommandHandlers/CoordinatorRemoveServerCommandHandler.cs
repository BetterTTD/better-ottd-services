using Akka.Util;
using OpenTTD.AdminClient.API.Abstractions;
using OpenTTD.AdminClient.API.Services;
using OpenTTD.AdminClient.Domain.Commands;
using OpenTTD.AdminClient.Domain.ValueObjects;

namespace OpenTTD.AdminClient.API.Domain.CommandHandlers;

public sealed class CoordinatorRemoveServerCommandHandler(ICoordinatorService coordinator,
        ILogger<CoordinatorRemoveServerCommandHandler> logger)
    : ICommandHandler<CoordinatorRemoveServer, ServerId>
{
    public async Task<Result<ServerId>> Handle(CoordinatorRemoveServer cmd, CancellationToken cancellationToken)
    {
        logger.LogInformation(
            "[CMD:{CmdName}] Data {Notification}", 
            nameof(CoordinatorRemoveServerCommandHandler), cmd);

        await coordinator.RemoveServerAsync(cmd.Id, cancellationToken);
        
        return Result.Success(cmd.Id);
    }
}