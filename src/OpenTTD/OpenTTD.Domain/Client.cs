using System.Net;

namespace OpenTTD.Domain;

public sealed record ClientId(byte Value);

public sealed record Client
{
    public ClientId Id { get; init; } = new(0);
    public NetworkAddress Host { get; init; } = new(IPAddress.None, 0);
    public string Name { get; init; } = "Unknown";
    public byte Language { get; init; }
    public long JoinDate { get; init; }
    public Company Company { get; init; } = Company.Spectator;
}