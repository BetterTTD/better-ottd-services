using OpenTTD.AdminClient.Networking.Enums;

namespace OpenTTD.StateService.Contracts.Events;

public sealed record ServerNetworkMessageReceived(
    Guid ServerId, 
    PacketType PacketType, 
    string Message);