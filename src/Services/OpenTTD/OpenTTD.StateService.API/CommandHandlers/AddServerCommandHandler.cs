using System.Net;
using DataAccess.Abstractions;
using MassTransit;
using OpenTTD.StateService.Contracts.Commands;
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

        await context.Send(new AdminClient.Contracts.Commands.AddServer(
            dbo.Id,
            dbo.Name,
            dbo.AdminName,
            dbo.IpAddress.ToString(), 
            dbo.Port, 
            dbo.Password, 
            "1.0"));
    }
}