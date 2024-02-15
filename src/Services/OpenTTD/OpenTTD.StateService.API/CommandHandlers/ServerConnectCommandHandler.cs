using MassTransit;
using OpenTTD.StateService.Contracts.Commands;

namespace OpenTTD.StateService.API.CommandHandlers;

public sealed class ServerConnectCommandHandler : IConsumer<ServerConnect>
{
    public Task Consume(ConsumeContext<ServerConnect> context)
    {
        throw new NotImplementedException();
    }
}