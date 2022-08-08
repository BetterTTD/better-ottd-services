using CSharpFunctionalExtensions;
using Domain.ValueObjects;
using MediatR;
using OpenTTD.Domain.Abstractions;
using OpenTTD.Services.Abstractions;

namespace OpenTTD.Domain.Commands.AddServer;

public sealed class AddServerCommandHandler : IRequestHandler<AddServerCommand, Result<ServerId>>
{
    private readonly IServerService _serverService;
    private readonly IServersSystemService _serversSystemService;

    public AddServerCommandHandler(IServerService serverService, IServersSystemService serversSystemService)
    {
        _serverService = serverService;
        _serversSystemService = serversSystemService;
    }

    public async Task<Result<ServerId>> Handle(AddServerCommand request, CancellationToken cancellationToken)
    {
        var result = await _serverService.AddServerAsync(request.Credentials, cancellationToken);

        if (!result.IsSuccess)
        {
            throw new InvalidOperationException();
        }
        
        var serverId = await _serversSystemService.AskSystemToAddServerAsync(request.Credentials, cancellationToken);
        
        return Result.Success(serverId);
    }
}