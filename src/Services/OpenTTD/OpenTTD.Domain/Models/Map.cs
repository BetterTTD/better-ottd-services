using OpenTTD.Networking.Enums;

namespace OpenTTD.Domain.Models;

public sealed record Map
{
    public string Name { get; init; } = "Unknown";
    public Landscape Landscape { get; init; }
    public ushort Width { get; init; }
    public ushort Height { get; init; }
}