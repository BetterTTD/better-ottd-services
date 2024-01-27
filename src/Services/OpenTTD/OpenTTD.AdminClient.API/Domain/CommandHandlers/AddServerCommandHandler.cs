using Akka.Util;
using OpenTTD.AdminClient.API.Domain.Abstractions;
using OpenTTD.AdminClient.API.Services;
using OpenTTD.AdminClient.Domain.Commands;
using OpenTTD.AdminClient.Domain.ValueObjects;

namespace OpenTTD.AdminClient.API.Domain.CommandHandlers;

public sealed class AddServerCommandHandler(ILogger<AddServerCommandHandler> logger, ICoordinatorService coordinator)
    : ICommandHandler<AddServer, ServerId>
{
    public async Task<Result<ServerId>> Handle(AddServer cmd, CancellationToken cancellationToken)
    {
        logger.LogInformation(
            "[CMD:{CmdName}] Data {Request}", 
            nameof(AddServer), cmd);

        return await coordinator.AskToAddServerAsync(cmd.Network, cancellationToken);
    }
}