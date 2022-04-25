using OpenTTD.Networking.AdminPort.Enums;
using OpenTTD.Networking.AdminPort.Messages.Base;

namespace OpenTTD.Networking.AdminPort.Messages.AdminServerClientJoin;

public sealed class AdminServerClientJoinTransformer : IMessageTransformer<AdminServerClientJoinMessage>
{
    public AdminPacketType PacketType => AdminPacketType.ADMIN_PACKET_SERVER_CLIENT_JOIN;

    public AdminServerClientJoinMessage Transform(Packet packet)
    {
        var msg = new AdminServerClientJoinMessage(packet.ReadU32());
        return msg;
    }
}