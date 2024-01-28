using Akka.Util;
using MediatR;

namespace OpenTTD.AdminClient.API.Abstractions;

public interface IHandlerBase<in TReq, TResp> : IRequestHandler<TReq, Result<TResp>>
    where TReq : IRequest<Result<TResp>>
{
    
}