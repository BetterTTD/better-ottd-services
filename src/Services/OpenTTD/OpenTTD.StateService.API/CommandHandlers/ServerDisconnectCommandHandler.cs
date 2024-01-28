using IntegrationCommands;
using MassTransit;

namespace OpenTTD.StateService.API.CommandHandlers;

public class ServerDisconnectCommandHandler : IConsumer<ServerDisconnect>
{
    public Task Consume(ConsumeContext<ServerDisconnect> context)
    {
        throw new NotImplementedException();
    }
}