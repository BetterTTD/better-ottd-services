using Akka.Util;
using Domain.ValueObjects;
using MediatR;
using OpenTTD.AdminClient.Services;

namespace OpenTTD.AdminClient.Domain.Commands.AddServer;

public class AddServerCommandHandler : IRequestHandler<AddServerCommand, Result<ServerId>>
{
    private readonly IServerService _serverService;

    public AddServerCommandHandler(IServerService serverService) => _serverService = serverService;

    public async Task<Result<ServerId>> Handle(AddServerCommand request, CancellationToken cancellationToken)
    {
        var result = await _serverService.AddServerAsync(request.Credentials, cancellationToken);
        return result;
    }
}