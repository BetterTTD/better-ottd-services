using OpenTTD.Networking.AdminPort.Enums;

namespace OpenTTD.Networking.AdminPort.Messages.ServerClientQuit;

public sealed class ServerClientQuitTransformer : IPacketTransformer<ServerClientQuit>
{
    public PacketType PacketType => PacketType.ADMIN_PACKET_SERVER_CLIENT_QUIT;

    public ServerClientQuit Transform(Packet packet)
    {
        var msg = new ServerClientQuit(packet.ReadU32());
        return msg;
    }
}