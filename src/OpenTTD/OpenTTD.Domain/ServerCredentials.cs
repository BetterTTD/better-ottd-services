using System.Net;

namespace OpenTTD.Domain;

public sealed record ServerCredentials
{
    public IPAddress IpAddress { get; init; } = null!;
    public int Port { get; init; }
    public string Password { get; init; } = null!;
}