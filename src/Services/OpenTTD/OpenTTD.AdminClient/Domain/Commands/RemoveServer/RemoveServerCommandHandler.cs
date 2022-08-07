using Akka.Util;
using Domain.ValueObjects;
using MediatR;
using OpenTTD.AdminClient.Services;

namespace OpenTTD.AdminClient.Domain.Commands.RemoveServer;

public sealed class RemoveServerCommandHandler : IRequestHandler<RemoveServerCommand, Result<ServerId>>
{
    private readonly IServerService _serverService;

    public RemoveServerCommandHandler(IServerService serverService) => _serverService = serverService;

    public async Task<Result<ServerId>> Handle(RemoveServerCommand request, CancellationToken cancellationToken)
    {
        var result = await _serverService.RemoveServerAsync(request.ServerId, cancellationToken);
        return result;
    }
}