using Akka.Util;
using OpenTTD.AdminClient.Domain.Abstractions;
using OpenTTD.AdminClient.Services;
using OpenTTD.AdminClientDomain.Commands;
using OpenTTD.AdminClientDomain.ValueObjects;

namespace OpenTTD.AdminClient.Domain.CommandHandlers;

public sealed class ConnectServerHandler : ICommandHandler<ConnectServer, ServerId>
{
    private readonly ICoordinatorService _coordinator;
    private readonly ILogger<ConnectServerHandler> _logger;

    public ConnectServerHandler(ICoordinatorService coordinator, ILogger<ConnectServerHandler> logger)
    {
        _coordinator = coordinator;
        _logger = logger;
    }

    public async Task<Result<ServerId>> Handle(ConnectServer cmd, CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "[CMD:{CmdName}] Data {Notification}", 
            nameof(ConnectServerHandler), cmd);

        await _coordinator.TellServerToConnectAsync(cmd.Id, cancellationToken);

        return Result.Success(cmd.Id);
    }
}