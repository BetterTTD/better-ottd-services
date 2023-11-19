using Akka.Util;
using OpenTTD.AdminClient.Domain.Abstractions;
using OpenTTD.AdminClient.Services;
using OpenTTD.AdminClientDomain.Commands;
using OpenTTD.AdminClientDomain.ValueObjects;

namespace OpenTTD.AdminClient.Domain.CommandHandlers;

public sealed class AddServerCommandHandler : ICommandHandler<AddServer, ServerId>
{
    private readonly ILogger<AddServerCommandHandler> _logger;
    private readonly ICoordinatorService _coordinator;

    public AddServerCommandHandler(ILogger<AddServerCommandHandler> logger, ICoordinatorService coordinator)
    {
        _logger = logger;
        _coordinator = coordinator;
    }

    public async Task<Result<ServerId>> Handle(AddServer cmd, CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "[CMD:{CmdName}] Data {Request}", 
            nameof(AddServer), cmd);

        return await _coordinator.AskToAddServerAsync(cmd.Credentials, cancellationToken);
    }
}