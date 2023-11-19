using OpenTTD.AdminClientDomain.Models;
using OpenTTD.AdminClientDomain.ValueObjects;

namespace OpenTTD.AdminClientDomain.Entities;

public sealed record Server : Entity<ServerId>
{
    public ServerName Name { get; init; } = new("Unknown");
    public ServerNetwork Network { get; init; } = new();
    public long Date { get; init; }
    public bool IsDedicated { get; init; }
    public Map Map { get; init; } = new();
    public List<Company> Companies { get; init; } = new() { Company.Spectator };
}