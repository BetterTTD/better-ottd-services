using System.Net;

namespace OpenTTD.AdminClientDomain.ValueObjects;

public sealed record NetworkAddress(IPAddress IpAddress, ServerPort Port)
{
    public override string ToString() => $"{IpAddress}:{Port.Value}";
}