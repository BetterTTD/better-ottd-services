using Akka.Util;
using OpenTTD.AdminClient.Domain.Abstractions;
using OpenTTD.AdminClient.Domain.Commands;
using OpenTTD.AdminClient.Domain.ValueObjects;
using OpenTTD.AdminClient.Services;

namespace OpenTTD.AdminClient.Domain.CommandHandlers;

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