using OpenTTD.Networking.Common;
using OpenTTD.Networking.Enums;

namespace OpenTTD.Networking.Messages.Inbound.ServerClientQuit;

public sealed class ServerClientQuitTransformer : IPacketTransformer<ServerClientQuit>
{
    public PacketType PacketType => PacketType.ADMIN_PACKET_SERVER_CLIENT_QUIT;

    public ServerClientQuit Transform(Packet packet)
    {
        var msg = new ServerClientQuit(packet.ReadU32());
        return msg;
    }
}