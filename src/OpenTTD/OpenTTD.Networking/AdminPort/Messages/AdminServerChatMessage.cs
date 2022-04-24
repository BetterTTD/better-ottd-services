using OpenTTD.Networking.AdminPort.Enums;

namespace OpenTTD.Networking.AdminPort.Messages;

public sealed record AdminServerChatMessage(
    NetworkAction NetworkAction,
    ChatDestination ChatDestination,
    uint ClientId,
    string Message,
    long Data) : IAdminMessage
{
    public AdminMessageType MessageType => AdminMessageType.ADMIN_PACKET_SERVER_CHAT;
}