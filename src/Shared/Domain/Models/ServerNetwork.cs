using Networking.Enums;

namespace Domain.Models;

public sealed record ServerNetwork
{
    public byte Version { get; init; }
    public string Revision { get; set; } = "Unknown";
    public Dictionary<UpdateType, UpdateFrequency> UpdateFrequencies { get; init; } = new();
}