using System.Net;
using DataAccess.Abstractions;
using IntegrationCommands;
using IntegrationEvents;
using MassTransit;
using OpenTTD.StateService.DataAccess.Entities;

namespace OpenTTD.StateService.API.CommandHandlers;

public sealed class AddServerCommandHandler(
    IGenericRepository<ServerEntity, Guid> repository) : IConsumer<AddServer>
{
    public async Task Consume(ConsumeContext<AddServer> context)
    {
        var msg = context.Message;
        
        var dbo = new ServerEntity
        {
            Name = msg.Name,
            AdminName = msg.AdminName,
            IpAddress = IPAddress.Parse(msg.IpAddress),
            Port = msg.Port,
            Password = msg.Password
        };
        
        await repository.CreateAsync(dbo);
        
        await repository.SaveAsync();

        await context.Publish(new ServerAdded(dbo.Id));
    }
}