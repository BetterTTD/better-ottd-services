using Akka.Util;
using Domain.ValueObjects;
using MediatR;
using OpenTTD.AdminClient.HostedServices;
using OpenTTD.AdminClient.Services;

namespace OpenTTD.AdminClient.Domain.Commands.RemoveServer;

public sealed class RemoveServerCommandHandler : IRequestHandler<RemoveServerCommand, Result<ServerId>>
{
    private readonly IServerService _serverService;
    private readonly IActorSystemService _actorSystemService;

    public RemoveServerCommandHandler(IServerService serverService, IActorSystemService actorSystemService)
    {
        _serverService = serverService;
        _actorSystemService = actorSystemService;
    }

    public async Task<Result<ServerId>> Handle(RemoveServerCommand request, CancellationToken cancellationToken)
    {
        var result = await _serverService.RemoveServerAsync(request.ServerId, cancellationToken);

        if (!result.IsSuccess)
        {
            throw new NotImplementedException();
        }

        _actorSystemService.TellSystemToRemoveServer(request.ServerId);
        
        return result;
    }
}