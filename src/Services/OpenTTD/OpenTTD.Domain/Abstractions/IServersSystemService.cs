using Domain.Models;
using Domain.ValueObjects;

namespace OpenTTD.Domain.Abstractions;

public interface IServersSystemService
{
    public Task<ServerId> AskSystemToAddServerAsync(ServerCredentials credentials, CancellationToken ct);
    public void TellServerToConnect(ServerId serverId);
    public void TellServerToDisconnect(ServerId serverId);
    public void TellSystemToRemoveServer(ServerId serverId);
}