using OpenTTD.Networking.Enums;

namespace OpenTTD.AdminClientDomain.ValueObjects;

public sealed record Network
{
    public byte Version { get; init; }
    public string Revision { get; set; } = "Unknown";
    public Dictionary<UpdateType, UpdateFrequency> UpdateFrequencies { get; init; } = new();
}