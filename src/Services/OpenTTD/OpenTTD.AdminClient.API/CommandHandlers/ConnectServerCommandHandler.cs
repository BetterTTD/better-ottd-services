using Akka.Util;
using OpenTTD.AdminClient.API.Abstractions;
using OpenTTD.AdminClient.API.Services;
using OpenTTD.AdminClient.Domain.Commands;
using OpenTTD.AdminClient.Domain.ValueObjects;

namespace OpenTTD.AdminClient.API.CommandHandlers;

public sealed class ConnectServerHandler(ICoordinatorService coordinator, ILogger<ConnectServerHandler> logger)
    : ICommandHandler<ConnectServer, ServerId>
{
    public async Task<Result<ServerId>> Handle(ConnectServer cmd, CancellationToken cancellationToken)
    {
        logger.LogInformation(
            "[CMD:{CmdName}] Data {Notification}", 
            nameof(ConnectServerHandler), cmd);

        await coordinator.TellServerToConnectAsync(cmd.Id, cancellationToken);

        return Result.Success(cmd.Id);
    }
}