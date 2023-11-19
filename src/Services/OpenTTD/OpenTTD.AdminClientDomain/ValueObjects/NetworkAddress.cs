using System.Net;

namespace OpenTTD.AdminClientDomain.ValueObjects;

public sealed record NetworkAddress(IPAddress IpAddress, int Port)
{
    public override string ToString() => $"{IpAddress}:{Port}";
}