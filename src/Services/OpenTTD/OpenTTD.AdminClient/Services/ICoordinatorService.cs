using Akka.Util;
using OpenTTD.AdminClient.Domain.ValueObjects;

namespace OpenTTD.AdminClient.Services;

public interface ICoordinatorService
{
    Task<Result<ServerId>> AskToAddServerAsync(ServerNetwork network, CancellationToken cts);
    Task TellServerToConnectAsync(ServerId id, CancellationToken cts);
    Task TellServerToDisconnectAsync(ServerId id, CancellationToken cts);
    Task RemoveServerAsync(ServerId id, CancellationToken cts);
}