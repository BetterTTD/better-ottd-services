using Akka.Util;
using MediatR;

namespace OpenTTD.AdminClient.Domain.Commands;

public interface ICommand<T> : IRequest<Result<T>>
{
    
}