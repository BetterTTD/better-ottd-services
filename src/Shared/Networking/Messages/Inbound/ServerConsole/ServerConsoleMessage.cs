using Networking.Enums;

namespace Networking.Messages.Inbound.ServerConsole;

public sealed record ServerConsoleMessage(
    string Origin, 
    string Message) : IMessage
{
    public PacketType PacketType => PacketType.ADMIN_PACKET_SERVER_CONSOLE;
}