using System.Net;

namespace OpenTTD.Domain;

public sealed record NetworkAddress(IPAddress IpAddress, int Port)
{
    public override string ToString() => $"{IpAddress}:{Port}";
}