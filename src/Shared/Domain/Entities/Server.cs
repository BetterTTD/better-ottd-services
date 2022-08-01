using Domain.Enums;
using Domain.Models;
using Domain.ValueObjects;

namespace Domain.Entities;

public sealed record Server : Entity<ServerId>
{
    public string Name { get; init; } = "Unknown";
    public long Date { get; init; }
    public bool IsDedicated { get; init; }
    public Map Map { get; init; } = new();
    public ServerNetwork Network { get; init; } = new();
    public ServerState State { get; init; } = ServerState.IDLE;
    public List<Company> Companies { get; init; } = new() { Company.Spectator };
}