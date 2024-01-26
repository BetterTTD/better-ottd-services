using OpenTTD.StateService.Domain.Enums;

namespace OpenTTD.StateService.Domain.Dtos;

public sealed record ServerMap
{
    public string Name { get; init; } = "Unknown";
    public Landscape Landscape { get; init; }
    public ushort Width { get; init; }
    public ushort Height { get; init; }
}