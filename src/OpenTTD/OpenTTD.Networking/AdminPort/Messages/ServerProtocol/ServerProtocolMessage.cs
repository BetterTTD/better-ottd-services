using OpenTTD.Networking.AdminPort.Enums;

namespace OpenTTD.Networking.AdminPort.Messages.ServerProtocol;

public sealed record ServerProtocolMessage(
    byte NetworkVersion, 
    Dictionary<AdminUpdateType, UpdateFrequency> AdminUpdateSettings) : IMessage
{
    public PacketType PacketType => PacketType.ADMIN_PACKET_SERVER_PROTOCOL;
}