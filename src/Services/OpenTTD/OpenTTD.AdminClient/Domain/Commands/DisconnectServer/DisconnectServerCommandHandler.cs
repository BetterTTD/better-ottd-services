using Domain.ValueObjects;
using MediatR;
using OpenTTD.AdminClient.HostedServices;

namespace OpenTTD.AdminClient.Domain.Commands.DisconnectServer;

public sealed class DisconnectServerCommandHandler : IRequestHandler<DisconnectServerCommand, ServerId>
{
    private readonly IActorSystemService _actorSystemService;

    public DisconnectServerCommandHandler(IActorSystemService actorSystemService) => 
        _actorSystemService = actorSystemService;

    public Task<ServerId> Handle(DisconnectServerCommand request, CancellationToken cancellationToken)
    {
        _actorSystemService.TellServerToDisconnect(request.ServerId);

        return Task.FromResult(request.ServerId);
    }
}