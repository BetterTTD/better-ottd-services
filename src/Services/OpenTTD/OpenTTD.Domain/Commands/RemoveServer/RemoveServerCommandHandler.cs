using CSharpFunctionalExtensions;
using Domain.ValueObjects;
using MediatR;
using OpenTTD.Domain.Abstractions;
using OpenTTD.Services.Abstractions;

namespace OpenTTD.Domain.Commands.RemoveServer;

public sealed class RemoveServerCommandHandler : IRequestHandler<RemoveServerCommand, Result<ServerId>>
{
    private readonly IServerService _serverService;
    private readonly IServersSystemService _serversSystemService;

    public RemoveServerCommandHandler(IServerService serverService, IServersSystemService serversSystemService)
    {
        _serverService = serverService;
        _serversSystemService = serversSystemService;
    }

    public async Task<Result<ServerId>> Handle(RemoveServerCommand request, CancellationToken cancellationToken)
    {
        var result = await _serverService.RemoveServerAsync(request.ServerId, cancellationToken);

        if (!result.IsSuccess)
        {
            throw new NotImplementedException();
        }

        _serversSystemService.TellSystemToRemoveServer(request.ServerId);
        
        return result.Value;
    }
}