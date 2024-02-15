using DataAccess.Abstractions;
using MassTransit;
using OpenTTD.StateService.Contracts.Commands;
using OpenTTD.StateService.Contracts.Events;
using OpenTTD.StateService.DataAccess.Entities;

namespace OpenTTD.StateService.API.CommandHandlers;

public sealed class RemoveServerCommandHandler(
    IGenericRepository<ServerEntity, Guid> repository) : IConsumer<RemoveServer>
{
    public async Task Consume(ConsumeContext<RemoveServer> context)
    {
        var msg = context.Message;

        var server = await repository.FindAsync(msg.ServerId);

        if (server is null)
        {
            throw new ArgumentNullException(nameof(server));
        }
        
        repository.Delete(server);

        await repository.SaveAsync();

        await context.Publish(new ServerRemoved(server.Id));
    }
}