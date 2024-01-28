using Akka.Util;
using OpenTTD.AdminClient.API.Abstractions;
using OpenTTD.AdminClient.API.Services;
using OpenTTD.AdminClient.Domain.Commands;
using OpenTTD.AdminClient.Domain.ValueObjects;

namespace OpenTTD.AdminClient.API.Domain.CommandHandlers;

public sealed class CoordinatorCoordinatorConnectServerHandler(ICoordinatorService coordinator, ILogger<CoordinatorCoordinatorConnectServerHandler> logger)
    : ICommandHandler<CoordinatorConnectServer, ServerId>
{
    public async Task<Result<ServerId>> Handle(CoordinatorConnectServer cmd, CancellationToken cancellationToken)
    {
        logger.LogInformation(
            "[CMD:{CmdName}] Data {Notification}", 
            nameof(CoordinatorCoordinatorConnectServerHandler), cmd);

        await coordinator.TellServerToConnectAsync(cmd.Id, cancellationToken);

        return Result.Success(cmd.Id);
    }
}