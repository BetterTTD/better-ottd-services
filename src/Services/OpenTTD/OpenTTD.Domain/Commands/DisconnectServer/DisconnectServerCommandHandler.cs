using Domain.ValueObjects;
using MediatR;
using OpenTTD.Domain.Abstractions;

namespace OpenTTD.Domain.Commands.DisconnectServer;

public sealed class DisconnectServerCommandHandler : IRequestHandler<DisconnectServerCommand, ServerId>
{
    private readonly IServersSystemService _serversSystemService;

    public DisconnectServerCommandHandler(IServersSystemService serversSystemService) => 
        _serversSystemService = serversSystemService;

    public Task<ServerId> Handle(DisconnectServerCommand request, CancellationToken cancellationToken)
    {
        _serversSystemService.TellServerToDisconnect(request.ServerId);

        return Task.FromResult(request.ServerId);
    }
}