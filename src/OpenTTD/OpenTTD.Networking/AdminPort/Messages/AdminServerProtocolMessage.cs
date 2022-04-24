using OpenTTD.Networking.AdminPort.Enums;

namespace OpenTTD.Networking.AdminPort.Messages;

public sealed record AdminServerProtocolMessage(
    byte NetworkVersion, 
    Dictionary<AdminUpdateType, UpdateFrequency> AdminUpdateSettings) : IAdminMessage
{
    public AdminMessageType MessageType => AdminMessageType.ADMIN_PACKET_SERVER_PROTOCOL;
}