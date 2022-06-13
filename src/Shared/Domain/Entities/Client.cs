using System.Net;
using Domain.ValueObjects;

namespace Domain.Entities;

public sealed record Client : Entity<ClientId>
{
    public IPAddress Address { get; init; } = IPAddress.None;
    public string Name { get; init; } = "Unknown";
    public byte Language { get; init; }
    public long JoinDate { get; init; }
    public Company Company { get; init; } = Company.Spectator;
}