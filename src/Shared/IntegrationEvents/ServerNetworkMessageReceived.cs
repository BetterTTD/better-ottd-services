using OpenTTD.AdminClient.Networking.Enums;

namespace IntegrationEvents;

public sealed record ServerNetworkMessageReceived(
    Guid ServerId, 
    PacketType PacketType, 
    string Message);