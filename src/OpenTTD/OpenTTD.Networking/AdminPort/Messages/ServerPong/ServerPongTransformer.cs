using OpenTTD.Networking.AdminPort.Enums;

namespace OpenTTD.Networking.AdminPort.Messages.ServerPong;

public sealed class ServerPongTransformer : IPacketTransformer<ServerPongMessage>
{
    public PacketType PacketType => PacketType.ADMIN_PACKET_SERVER_PONG;

    public ServerPongMessage Transform(Packet packet)
    {
        var msg = new ServerPongMessage(packet.ReadU32());
        return msg;
    }
}