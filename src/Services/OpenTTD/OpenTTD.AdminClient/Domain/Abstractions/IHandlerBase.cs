using Akka.Util;
using MediatR;

namespace OpenTTD.AdminClient.Domain.Abstractions;

public interface IHandlerBase<in TReq, TResp> : IRequestHandler<TReq, Result<TResp>>
    where TReq : IRequest<Result<TResp>>
{
    
}