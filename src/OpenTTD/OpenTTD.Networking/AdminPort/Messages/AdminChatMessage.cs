using OpenTTD.Networking.AdminPort.Enums;

namespace OpenTTD.Networking.AdminPort.Messages;

public sealed record AdminChatMessage(
    NetworkAction NetworkAction, 
    ChatDestination ChatDestination, 
    uint Destination, 
    string Message) : IAdminMessage
{
    public AdminMessageType MessageType => AdminMessageType.ADMIN_PACKET_ADMIN_CHAT;
}