using OpenTTD.AdminClientDomain.ValueObjects;

namespace OpenTTD.AdminClientDomain.Entities;

public sealed record Server : Entity<ServerId>
{
    public ServerName Name { get; init; } = new("Unknown");
    public ServerNetwork ServerNetwork { get; init; } = new();
    public Network Network { get; init; } = new();
    public long Date { get; init; }
    public bool IsDedicated { get; init; }
    public Map Map { get; init; } = new();
    public List<Company> Companies { get; init; } = new() { Company.Spectator };
}