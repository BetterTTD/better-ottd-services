using Akka.Util;
using MediatR;
using OpenTTD.AdminClient.Services;
using OpenTTD.Domain.Commands;
using OpenTTD.Domain.ValueObjects;

namespace OpenTTD.AdminClient.Domain.CommandHandlers;

public sealed class AddServerCommandHandler : IRequestHandler<AddServer, Result<ServerId>>
{
    private readonly ILogger<AddServerCommandHandler> _logger;
    private readonly ICoordinatorService _coordinator;

    public AddServerCommandHandler(ILogger<AddServerCommandHandler> logger, ICoordinatorService coordinator)
    {
        _logger = logger;
        _coordinator = coordinator;
    }

    public async Task<Result<ServerId>> Handle(AddServer request, CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "[CMD:{CmdName}] Data {Request}", 
            nameof(AddServer), request);

        return await _coordinator.AskToAddServerAsync(request.Credentials, cancellationToken);
    }
}