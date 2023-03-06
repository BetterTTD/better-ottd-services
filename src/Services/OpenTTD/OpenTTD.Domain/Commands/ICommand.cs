using Akka.Util;
using MediatR;

namespace OpenTTD.Domain.Commands;

public interface ICommand<T> : IRequest<Result<T>>
{
    
}