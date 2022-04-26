using OpenTTD.Networking.Common;
using OpenTTD.Networking.Enums;

namespace OpenTTD.Networking.Messages.Inbound.ServerClientQuit;

public sealed class ServerClientQuitTransformer : IPacketTransformer<ServerClientQuitMessage>
{
    public PacketType PacketType => PacketType.ADMIN_PACKET_SERVER_CLIENT_QUIT;

    public ServerClientQuitMessage Transform(Packet packet)
    {
        var msg = new ServerClientQuitMessage(packet.ReadU32());
        return msg;
    }
}