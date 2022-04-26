using OpenTTD.Networking.Common;
using OpenTTD.Networking.Enums;

namespace OpenTTD.Networking.Messages.Inbound.ServerClientQuit;

public sealed class ServerClientQuitTransformer : IPacketTransformer
{
    public PacketType PacketType => PacketType.ADMIN_PACKET_SERVER_CLIENT_QUIT;

    public IMessage Transform(Packet packet)
    {
        var msg = new ServerClientQuitMessage(packet.ReadU32());
        return msg;
    }
}