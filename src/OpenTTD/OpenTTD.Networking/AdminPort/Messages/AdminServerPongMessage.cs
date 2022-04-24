using OpenTTD.Networking.AdminPort.Enums;

namespace OpenTTD.Networking.AdminPort.Messages;

public sealed record AdminServerPongMessage(uint Argument) : IAdminMessage
{
    public AdminMessageType MessageType => AdminMessageType.ADMIN_PACKET_SERVER_PONG;
}