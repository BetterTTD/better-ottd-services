using OpenTTD.StateService.Domain.Enums;

namespace OpenTTD.StateService.Domain.Dtos;

public class ServerNetworkConfiguration
{
    public byte Version { get; init; }
    public string Revision { get; set; } = "Unknown";
    public Dictionary<ServerUpdateType, ServerUpdateFrequency> UpdateFrequencies { get; init; } = new();
}