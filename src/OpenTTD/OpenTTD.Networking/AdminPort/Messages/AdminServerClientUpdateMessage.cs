using OpenTTD.Networking.AdminPort.Enums;

namespace OpenTTD.Networking.AdminPort.Messages;

public sealed record AdminServerClientUpdateMessage(
    uint ClientId,
    string ClientName,
    byte PlayingAs) : IAdminMessage
{
    public AdminMessageType MessageType => AdminMessageType.ADMIN_PACKET_SERVER_CLIENT_UPDATE;
}