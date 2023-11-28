﻿using Akka.Util;
using OpenTTD.AdminClient.Domain.Abstractions;
using OpenTTD.AdminClient.Services;
using OpenTTD.AdminClientDomain.Commands;
using OpenTTD.AdminClientDomain.ValueObjects;

namespace OpenTTD.AdminClient.Domain.CommandHandlers;

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