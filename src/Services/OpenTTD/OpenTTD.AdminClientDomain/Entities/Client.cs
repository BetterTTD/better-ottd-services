using System.Net;
using OpenTTD.AdminClientDomain.ValueObjects;

namespace OpenTTD.AdminClientDomain.Entities;

public sealed record Client : Entity<ClientId>
{
    public IPAddress Address { get; init; } = IPAddress.None;
    public string Name { get; init; } = "Unknown";
    public byte Language { get; init; }
    public long JoinDate { get; init; }
}