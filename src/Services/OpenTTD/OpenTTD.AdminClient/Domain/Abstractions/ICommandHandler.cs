using OpenTTD.AdminClientDomain.Commands;

namespace OpenTTD.AdminClient.Domain.Abstractions;

public interface ICommandHandler<TCommand, TResponse> : IHandlerBase<TCommand, TResponse>
    where TCommand : ICommand<TResponse>
{
    
}