using Networking.Common;
using Networking.Enums;

namespace Networking.Messages.Inbound.ServerClientJoin;

public sealed class ServerClientJoinTransformer : IPacketTransformer
{
    public PacketType PacketType => PacketType.ADMIN_PACKET_SERVER_CLIENT_JOIN;

    public IMessage Transform(Packet packet)
    {
        var msg = new ServerClientJoinMessage(packet.ReadU32());
        return msg;
    }
}