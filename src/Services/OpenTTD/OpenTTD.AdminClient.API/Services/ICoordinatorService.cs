using Akka.Util;
using OpenTTD.AdminClient.Domain.ValueObjects;

namespace OpenTTD.AdminClient.API.Services;

public interface ICoordinatorService
{
    Task<Result<ServerId>> AskToAddServerAsync(ServerId id, ServerNetwork network, CancellationToken cts);
    Task TellToConnectServerAsync(ServerId id, CancellationToken cts);
    Task TellToDisconnectServerAsync(ServerId id, CancellationToken cts);
    Task TellToRemoveServerAsync(ServerId id, CancellationToken cts);
}