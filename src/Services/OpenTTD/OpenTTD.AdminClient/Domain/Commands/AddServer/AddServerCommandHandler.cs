using Akka.Util;
using Domain.ValueObjects;
using MediatR;
using OpenTTD.AdminClient.HostedServices;
using OpenTTD.AdminClient.Services;

namespace OpenTTD.AdminClient.Domain.Commands.AddServer;

public sealed class AddServerCommandHandler : IRequestHandler<AddServerCommand, Result<ServerId>>
{
    private readonly IServerService _serverService;
    private readonly IActorSystemService _actorSystemService;

    public AddServerCommandHandler(IServerService serverService, IActorSystemService actorSystemService)
    {
        _serverService = serverService;
        _actorSystemService = actorSystemService;
    }

    public async Task<Result<ServerId>> Handle(AddServerCommand request, CancellationToken cancellationToken)
    {
        var result = await _serverService.AddServerAsync(request.Credentials, cancellationToken);

        if (!result.IsSuccess)
        {
            throw new InvalidOperationException();
        }
        
        var serverId = await _actorSystemService.AskSystemToAddServerAsync(request.Credentials, cancellationToken);
        
        return Result.Success(serverId);
    }
}