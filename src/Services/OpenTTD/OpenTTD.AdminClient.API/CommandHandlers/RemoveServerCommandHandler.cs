using Akka.Util;
using OpenTTD.AdminClient.API.Abstractions;
using OpenTTD.AdminClient.API.Services;
using OpenTTD.AdminClient.Domain.Commands;
using OpenTTD.AdminClient.Domain.ValueObjects;

namespace OpenTTD.AdminClient.API.CommandHandlers;

public sealed class RemoveServerCommandHandler(ICoordinatorService coordinator,
        ILogger<RemoveServerCommandHandler> logger)
    : ICommandHandler<RemoveServer, ServerId>
{
    public async Task<Result<ServerId>> Handle(RemoveServer cmd, CancellationToken cancellationToken)
    {
        logger.LogInformation(
            "[CMD:{CmdName}] Data {Notification}", 
            nameof(RemoveServerCommandHandler), cmd);

        await coordinator.RemoveServerAsync(cmd.Id, cancellationToken);
        
        return Result.Success(cmd.Id);
    }
}