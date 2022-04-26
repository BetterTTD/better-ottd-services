using System.Net;

namespace OpenTTD.Domain;

public sealed record ServerAddress(IPAddress IpAddress, int Port)
{
    public override string ToString() => $"{IpAddress}:{Port}";
}

public sealed record ServerCredentials
{
    public ServerAddress ServerAddress { get; init; }
    public string Name { get; init; }
    public string Version { get; init; }
    public string Password { get; init; }
};