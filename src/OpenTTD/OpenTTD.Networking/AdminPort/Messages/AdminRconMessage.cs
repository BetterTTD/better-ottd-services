using OpenTTD.Networking.AdminPort.Enums;

namespace OpenTTD.Networking.AdminPort.Messages;

public sealed record AdminRconMessage(string Command) : IAdminMessage
{
    public AdminMessageType MessageType => AdminMessageType.ADMIN_PACKET_ADMIN_RCON;
}