using OpenTTD.Networking.AdminPort.Enums;

namespace OpenTTD.Networking.AdminPort.Messages.ServerClientJoin;

public sealed class ServerClientJoinTransformer : IPacketTransformer<ServerClientJoinMessage>
{
    public PacketType PacketType => PacketType.ADMIN_PACKET_SERVER_CLIENT_JOIN;

    public ServerClientJoinMessage Transform(Packet packet)
    {
        var msg = new ServerClientJoinMessage(packet.ReadU32());
        return msg;
    }
}