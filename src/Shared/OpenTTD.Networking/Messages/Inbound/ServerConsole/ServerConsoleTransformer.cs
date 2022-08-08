using OpenTTD.Networking.Common;
using OpenTTD.Networking.Enums;

namespace OpenTTD.Networking.Messages.Inbound.ServerConsole;

public sealed class ServerConsoleTransformer : IPacketTransformer
{
    public PacketType PacketType => PacketType.ADMIN_PACKET_SERVER_CONSOLE;

    public IMessage Transform(Packet packet)
    {
        var msg = new ServerConsoleMessage(packet.ReadString(), packet.ReadString());
        return msg;
    }
}