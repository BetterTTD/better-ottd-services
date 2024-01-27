using OpenTTD.AdminClient.Networking.Enums;

namespace IntegrationEvents;

public sealed record ServerNetworkMessageReceived
{
    public required Guid ServerId { get; init; }
    public required PacketType PacketType { get; init; }
    public required string Message { get; init; }
}