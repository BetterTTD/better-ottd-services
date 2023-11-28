using Akka.Util;
using OpenTTD.AdminClient.Domain.Abstractions;
using OpenTTD.AdminClient.Services;
using OpenTTD.AdminClientDomain.Commands;
using OpenTTD.AdminClientDomain.ValueObjects;

namespace OpenTTD.AdminClient.Domain.CommandHandlers;

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