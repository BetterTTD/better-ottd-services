using System.Net;

namespace OpenTTD.Domain;

public sealed record ServerAddress(IPAddress IpAddress, int Port)
{
    public override string ToString() => $"{IpAddress}:{Port}";
}

public sealed record ServerCredentials(ServerAddress ServerAddress, string AdminPassword);