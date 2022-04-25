using OpenTTD.Networking.AdminPort.Enums;
using OpenTTD.Networking.AdminPort.Messages.Base;

namespace OpenTTD.Networking.AdminPort.Messages.AdminServerConsole;

public sealed record AdminServerConsoleMessage(
    string Origin, 
    string Message) : IAdminMessage
{
    public AdminPacketType PacketType => AdminPacketType.ADMIN_PACKET_SERVER_CONSOLE;
}