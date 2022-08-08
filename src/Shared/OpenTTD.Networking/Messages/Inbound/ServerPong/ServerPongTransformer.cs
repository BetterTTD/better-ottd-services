using OpenTTD.Networking.Common;
using OpenTTD.Networking.Enums;

namespace OpenTTD.Networking.Messages.Inbound.ServerPong;

public sealed class ServerPongTransformer : IPacketTransformer
{
    public PacketType PacketType => PacketType.ADMIN_PACKET_SERVER_PONG;

    public IMessage Transform(Packet packet)
    {
        var msg = new ServerPongMessage(packet.ReadU32());
        return msg;
    }
}