using Akka.Util;
using MediatR;
using OpenTTD.AdminClient.Services;
using OpenTTD.Domain.Commands;
using OpenTTD.Domain.ValueObjects;

namespace OpenTTD.AdminClient.Domain.CommandHandlers;

public sealed class AddServerCommandHandler : IRequestHandler<AddServer, Result<ServerId>>
{
    private readonly ICoordinatorService _coordinator;

    public AddServerCommandHandler(ICoordinatorService coordinator)
    {
        _coordinator = coordinator;
    }

    public async Task<Result<ServerId>> Handle(AddServer request, CancellationToken cancellationToken)
    {
        return await _coordinator.AskToAddServerAsync(request.Credentials, cancellationToken);
    }
}