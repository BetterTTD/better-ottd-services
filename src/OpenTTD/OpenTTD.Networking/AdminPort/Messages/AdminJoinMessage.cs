using OpenTTD.Networking.AdminPort.Enums;

namespace OpenTTD.Networking.AdminPort.Messages;

public sealed record AdminJoinMessage(
    string Password, 
    string AdminName, 
    string AdminVersion) : IAdminMessage
{
    public AdminMessageType MessageType => AdminMessageType.ADMIN_PACKET_ADMIN_JOIN;
}