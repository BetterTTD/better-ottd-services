using OpenTTD.AdminClient.Domain.Commands;

namespace OpenTTD.AdminClient.API.Abstractions;

public interface ICommandHandler<TCommand, TResponse> : IHandlerBase<TCommand, TResponse>
    where TCommand : ICommand<TResponse>
{
    
}