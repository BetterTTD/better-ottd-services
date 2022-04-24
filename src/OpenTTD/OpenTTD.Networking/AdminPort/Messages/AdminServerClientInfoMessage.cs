using OpenTTD.Networking.AdminPort.Enums;

namespace OpenTTD.Networking.AdminPort.Messages;

public sealed record AdminServerClientInfoMessage(
    uint ClientId,
    string Hostname,
    string ClientName,
    byte Language,
    long JoinDate,
    byte PlayingAs) : IAdminMessage
{
    public AdminMessageType MessageType => AdminMessageType.ADMIN_PACKET_SERVER_CLIENT_INFO;
}