using OpenTTD.Networking.AdminPort.Enums;

namespace OpenTTD.Networking.AdminPort.Messages.ServerConsole;

public sealed record ServerConsoleMessage(
    string Origin, 
    string Message) : IMessage
{
    public PacketType PacketType => PacketType.ADMIN_PACKET_SERVER_CONSOLE;
}