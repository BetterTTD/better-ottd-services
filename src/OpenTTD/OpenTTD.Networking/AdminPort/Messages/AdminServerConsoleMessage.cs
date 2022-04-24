using OpenTTD.Networking.AdminPort.Enums;

namespace OpenTTD.Networking.AdminPort.Messages;

public sealed record AdminServerConsoleMessage(
    string Origin, 
    string Message) : IAdminMessage
{
    public AdminMessageType MessageType => AdminMessageType.ADMIN_PACKET_SERVER_CONSOLE;
}