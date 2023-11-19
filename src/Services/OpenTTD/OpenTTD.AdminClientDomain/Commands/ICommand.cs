using Akka.Util;
using MediatR;

namespace OpenTTD.AdminClientDomain.Commands;

public interface ICommand<T> : IRequest<Result<T>>
{
    
}