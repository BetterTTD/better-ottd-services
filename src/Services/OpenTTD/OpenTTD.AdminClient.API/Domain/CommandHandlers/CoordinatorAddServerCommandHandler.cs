using Akka.Util;
using OpenTTD.AdminClient.API.Abstractions;
using OpenTTD.AdminClient.API.Services;
using OpenTTD.AdminClient.Domain.Commands;
using OpenTTD.AdminClient.Domain.ValueObjects;

namespace OpenTTD.AdminClient.API.Domain.CommandHandlers;

public sealed class CoordinatorAddServerCommandHandler(ILogger<CoordinatorAddServerCommandHandler> logger, ICoordinatorService coordinator)
    : ICommandHandler<CoordinatorAddServer, ServerId>
{ 
    public async Task<Result<ServerId>> Handle(CoordinatorAddServer cmd, CancellationToken cancellationToken)
    {
        logger.LogInformation(
            "[CMD:{CmdName}] Data {Request}", 
            nameof(CoordinatorAddServer), cmd);

        return await coordinator.AskToAddServerAsync(cmd.Id, cmd.Network, cancellationToken);
    }
}