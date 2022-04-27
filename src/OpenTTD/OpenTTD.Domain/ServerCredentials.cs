using System.Net;

namespace OpenTTD.Domain;

public sealed record ServerCredentials
{
    public NetworkAddress NetworkAddress { get; init; } = new(IPAddress.None, 0);
    public string Name { get; init; } = string.Empty;
    public string Version { get; init; } = string.Empty;
    public string Password { get; init; } = string.Empty;
};