using Domain.ValueObjects;
using MediatR;
using OpenTTD.AdminClient.HostedServices;

namespace OpenTTD.AdminClient.Domain.Commands.ConnectServer;

public sealed class ConnectServerCommandHandler : IRequestHandler<ConnectServerCommand, ServerId>
{
    private readonly IActorSystemService _actorSystemService;

    public ConnectServerCommandHandler(IActorSystemService actorSystemService)
    {
        _actorSystemService = actorSystemService;
    }

    public Task<ServerId> Handle(ConnectServerCommand request, CancellationToken cancellationToken)
    {
        _actorSystemService.TellServerToConnect(request.ServerId);

        return Task.FromResult(request.ServerId);
    }
}