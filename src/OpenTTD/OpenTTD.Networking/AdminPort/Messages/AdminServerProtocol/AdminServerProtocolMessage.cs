using OpenTTD.Networking.AdminPort.Enums;
using OpenTTD.Networking.AdminPort.Messages.Base;

namespace OpenTTD.Networking.AdminPort.Messages.AdminServerProtocol;

public sealed record AdminServerProtocolMessage(
    byte NetworkVersion, 
    Dictionary<AdminUpdateType, UpdateFrequency> AdminUpdateSettings) : IAdminMessage
{
    public AdminPacketType PacketType => AdminPacketType.ADMIN_PACKET_SERVER_PROTOCOL;
}