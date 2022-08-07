using Domain.Enums;
using Domain.Models;
using Domain.ValueObjects;

namespace Domain.Entities;

public sealed record Server : Entity<ServerId>
{
    public ServerName Name { get; init; } = new("Unknown");
    public ServerNetwork Network { get; init; } = new();
    public ServerState State { get; init; } = ServerState.IDLE;
    public long Date { get; init; }
    public bool IsDedicated { get; init; }
    public Map Map { get; init; } = new();
    public List<Company> Companies { get; init; } = new() { Company.Spectator };
}