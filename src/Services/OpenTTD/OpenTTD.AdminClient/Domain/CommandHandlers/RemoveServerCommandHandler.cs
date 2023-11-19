using Akka.Util;
using OpenTTD.AdminClient.Domain.Abstractions;
using OpenTTD.AdminClient.Services;
using OpenTTD.AdminClientDomain.Commands;
using OpenTTD.AdminClientDomain.ValueObjects;

namespace OpenTTD.AdminClient.Domain.CommandHandlers;

public sealed class RemoveServerCommandHandler : ICommandHandler<RemoveServer, ServerId>
{
    private readonly ICoordinatorService _coordinator;
    private readonly ILogger<RemoveServerCommandHandler> _logger;

    public RemoveServerCommandHandler(ICoordinatorService coordinator, ILogger<RemoveServerCommandHandler> logger)
    {
        _coordinator = coordinator;
        _logger = logger;
    }

    public async Task<Result<ServerId>> Handle(RemoveServer cmd, CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "[CMD:{CmdName}] Data {Notification}", 
            nameof(RemoveServerCommandHandler), cmd);

        await _coordinator.RemoveServerAsync(cmd.Id, cancellationToken);
        
        return Result.Success(cmd.Id);
    }
}