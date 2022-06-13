using System.Net;

namespace Domain.ValueObjects;

public sealed record NetworkAddress(IPAddress IpAddress, int Port)
{
    public override string ToString() => $"{IpAddress}:{Port}";
}