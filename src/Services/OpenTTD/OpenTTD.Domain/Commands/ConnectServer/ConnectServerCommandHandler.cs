using Domain.ValueObjects;
using MediatR;
using OpenTTD.Domain.Abstractions;

namespace OpenTTD.Domain.Commands.ConnectServer;

public sealed class ConnectServerCommandHandler : IRequestHandler<ConnectServerCommand, ServerId>
{
    private readonly IServersSystemService _serversSystemService;

    public ConnectServerCommandHandler(IServersSystemService serversSystemService)
    {
        _serversSystemService = serversSystemService;
    }

    public Task<ServerId> Handle(ConnectServerCommand request, CancellationToken cancellationToken)
    {
        _serversSystemService.TellServerToConnect(request.ServerId);

        return Task.FromResult(request.ServerId);
    }
}