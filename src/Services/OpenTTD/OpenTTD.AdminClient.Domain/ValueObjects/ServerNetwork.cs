using System.Net;

namespace OpenTTD.AdminClient.Domain.ValueObjects;

public sealed record ServerNetwork
{
    public NetworkAddress NetworkAddress { get; init; } = new(IPAddress.None, new ServerPort(default));
    public ServerName Name { get; init; } = new(string.Empty);
    public ServerVersion Version { get; init; } = new(string.Empty);
    public ServerPassword Password { get; init; } = new(string.Empty);
}