using System.Net;
using MassTransit;
using OpenTTD.AdminClient.API.Services;
using OpenTTD.AdminClient.Contracts.Commands;
using OpenTTD.AdminClient.Domain.ValueObjects;
using OpenTTD.StateService.Contracts.Events;

namespace OpenTTD.AdminClient.API.Integration.CommandHandlers;

public sealed class AddServerCommandHandler(ICoordinatorService coordinator) : IConsumer<AddServer>
{
    public async Task Consume(ConsumeContext<AddServer> context)
    {
        var cmd = context.Message;

        var serverId = new ServerId(cmd.ServerId);
        var network = new ServerNetwork
        {
            AdminName = new ServerAdminName(cmd.AdminName),
            Password = new ServerPassword(cmd.Password),
            NetworkAddress = new NetworkAddress(IPAddress.Parse(cmd.IpAddress), new ServerPort(cmd.Port)),
            Version = new ServerVersion("1.0")
        };

        var result = await coordinator.AskToAddServerAsync(serverId, network, CancellationToken.None);

        if (!result.IsSuccess)
        {
            throw new NotImplementedException();
        }

        await context.Publish(new ServerAdded(cmd.ServerId));
    }
}