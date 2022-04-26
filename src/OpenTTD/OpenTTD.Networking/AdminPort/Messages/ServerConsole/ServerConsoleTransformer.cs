using OpenTTD.Networking.AdminPort.Enums;

namespace OpenTTD.Networking.AdminPort.Messages.ServerConsole;

public sealed class ServerConsoleTransformer : IPacketTransformer<ServerConsoleMessage>
{
    public PacketType PacketType => PacketType.ADMIN_PACKET_SERVER_CONSOLE;

    public ServerConsoleMessage Transform(Packet packet)
    {
        var msg = new ServerConsoleMessage(packet.ReadString(), packet.ReadString());
        return msg;
    }
}