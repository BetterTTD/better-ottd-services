using Grpc.Core;
using MediatR;

namespace OpenTTD.AdminClient.Services;

public sealed class ServerService : Server.ServerBase
{
    private readonly ILogger<ServerService> _logger;
    private readonly IMediator _mediator;

    public ServerService(IMediator mediator, ILogger<ServerService> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    public override Task<ServerAddedResponse> AddServer(ServerCredentialsRequest request, ServerCallContext context)
    {
        return base.AddServer(request, context);
    }
}