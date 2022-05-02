using System.Net;
using OpenTTD.Domain.ValueObjects;

namespace OpenTTD.Domain.Entities;

public sealed record Client : Entity<ClientId>
{
    public NetworkAddress Host { get; init; } = new(IPAddress.None, 0);
    public string Name { get; init; } = "Unknown";
    public byte Language { get; init; }
    public long JoinDate { get; init; }
    public Company Company { get; init; } = Company.Spectator;
}